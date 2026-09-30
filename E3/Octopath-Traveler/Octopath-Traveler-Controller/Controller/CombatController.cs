using Octopath_Traveler_View;

namespace Octopath_Traveler;

public class CombatController
{
    private const int BasicAttackChoice = 1;
    private const int UseSkillChoice = 2;
    private const int DefendChoice = 3;
    private const int FleeChoice = 4;

    private readonly List<Traveler> _travelers;
    private readonly List<Beast> _beasts;
    private readonly CombatState _state;
    private readonly CombatReporter _reporter;
    private readonly CombatResolver _resolver;
    private readonly ActionSelector _selector;
    private readonly TurnQueue _turnQueue = new();
    private readonly Dictionary<int, ITravelerAction> _travelerActions;

    private int _round;
    private bool _combatFinished;
    private List<Unit> _currentRoundOrder = new();
    private int _currentRoundIndex;

    public CombatController(View view, List<Traveler> travelers, List<Beast> beasts)
    {
        _travelers = travelers;
        _beasts = beasts;
        _state = new CombatState(travelers, beasts);
        _reporter = new CombatReporter(view, _state);
        _resolver = new CombatResolver(_state);
        _selector = new ActionSelector(view, _state, _reporter);
        _travelerActions = BuildTravelerActions();
    }

    public void Run()
    {
        ApplyCombatStartPassives();
        while (!IsCombatFinished())
        {
            AdvanceRound();
            PlayRound();
            if (!IsCombatFinished())
                EndRound();
        }
    }

    internal void ResolveBasicAttack(Traveler traveler, BasicAttackRequest request)
    {
        IReadOnlyList<SkillOutcome> outcomes = _resolver.ResolveBasicAttack(traveler, request);
        _reporter.AnnounceBasicAttack(traveler.Name);
        Announce(outcomes);
        FinishCombatIfNeeded();
    }

    internal void ResolveSkill(Traveler traveler, SkillRequest request)
    {
        IReadOnlyList<SkillOutcome> outcomes = _resolver.ResolveSkill(traveler, request);
        _reporter.AnnounceSkillUse(traveler.Name, request.Skill.Name);
        Announce(outcomes);
        FinishCombatIfNeeded();
    }

    internal void Defend(Traveler traveler)
        => traveler.BeginDefending(_round);

    internal void Flee()
    {
        _reporter.AnnounceFlee();
        _reporter.AnnounceEnemyWins();
        _combatFinished = true;
    }

    private static Dictionary<int, ITravelerAction> BuildTravelerActions()
        => new()
        {
            [BasicAttackChoice] = new BasicAttackAction(),
            [UseSkillChoice] = new UseSkillAction(),
            [DefendChoice] = new DefendAction(),
            [FleeChoice] = new FleeAction()
        };

    private bool IsCombatFinished()
        => _combatFinished || _state.AreAllBeastsDead || _state.AreAllTravelersDead;

    private void FinishCombatIfNeeded()
    {
        if (_state.AreAllBeastsDead)
            AnnouncePlayerWin();
        else if (_state.AreAllTravelersDead)
            AnnounceEnemyWin();
    }

    private void AnnouncePlayerWin()
    {
        _reporter.AnnouncePlayerWins();
        _combatFinished = true;
    }

    private void AnnounceEnemyWin()
    {
        _reporter.AnnounceEnemyWins();
        _combatFinished = true;
    }

    private void ApplyCombatStartPassives()
    {
        foreach (Traveler traveler in _travelers)
            foreach (PassiveSkill passive in traveler.PassiveSkills)
                passive.OnCombatStart(traveler);
    }

    private void AdvanceRound()
    {
        _round++;
        _state.Round = _round;
        _reporter.AnnounceRoundStart(_round);
        _currentRoundOrder = _turnQueue.BuildOrder(_travelers, _beasts, _round);
        RecoverBeasts();
        _currentRoundIndex = 0;
    }

    private void RecoverBeasts()
    {
        foreach (Beast beast in _beasts.Where(
                     beast => beast.IsAlive && beast.RecoversInRound(_round)))
            beast.RecoverFromBreakingPoint();
    }

    private void PlayRound()
    {
        PlayCurrentOrder();
        if (!IsCombatFinished())
            PlayExtraTurns();
    }

    private void PlayCurrentOrder()
    {
        while (!IsCombatFinished())
        {
            Unit? unit = GetNextActiveUnit();
            if (unit is null)
                break;

            _reporter.ShowTurnState(GetRemainingUnitsInCurrentRound(), NextRoundOrder());
            PlayTurn(unit);
            _currentRoundIndex++;
        }
    }

    // Los turnos extra de Patience se juegan antes de las curas de fin de ronda y antes de otorgar BP.
    private void PlayExtraTurns()
    {
        List<Unit> extraTurns = PatienceExtraTurns();
        if (extraTurns.Count == 0)
            return;

        foreach (Unit unit in extraTurns)
            _reporter.AnnounceExtraTurn(unit.Name);
        _currentRoundOrder = extraTurns;
        _currentRoundIndex = 0;
        PlayCurrentOrder();
    }

    private List<Unit> NextRoundOrder()
        => _turnQueue.BuildOrder(_travelers, _beasts, _round + 1);

    private List<Unit> PatienceExtraTurns()
        => _travelers
            .Where(traveler => traveler.IsAlive && HasPatienceExtraTurn(traveler))
            .Cast<Unit>()
            .ToList();

    private static bool HasPatienceExtraTurn(Traveler traveler)
        => traveler.PassiveSkills.Any(passive => passive.GrantsExtraTurn(traveler));

    private Unit? GetNextActiveUnit()
    {
        while (_currentRoundIndex < _currentRoundOrder.Count &&
               !CanAct(_currentRoundOrder[_currentRoundIndex]))
            _currentRoundIndex++;

        return _currentRoundIndex < _currentRoundOrder.Count
            ? _currentRoundOrder[_currentRoundIndex]
            : null;
    }

    private bool CanAct(Unit unit)
        => unit.IsAlive && (unit is not Beast beast || !beast.IsBrokenDuring(_round));

    private List<Unit> GetRemainingUnitsInCurrentRound()
        => _currentRoundOrder.Skip(_currentRoundIndex).Where(CanAct).ToList();

    private void PlayTurn(Unit unit)
    {
        if (unit is Traveler traveler)
            PlayTravelerTurn(traveler);
        else if (unit is Beast beast)
            PlayBeastTurn(beast);
    }

    private void PlayTravelerTurn(Traveler traveler)
    {
        while (!IsCombatFinished())
        {
            int choice = _selector.SelectAction(traveler.Name);
            if (!_travelerActions.TryGetValue(choice, out ITravelerAction? action))
                continue;

            if (action.Execute(_selector, this, traveler) != TurnOutcome.Cancelled)
                return;
        }
    }

    private void PlayBeastTurn(Beast beast)
    {
        IReadOnlyList<SkillOutcome> outcomes = _resolver.ResolveBeastTurn(beast);
        _reporter.AnnounceSkillUse(beast.Name, beast.Skill.Name);
        Announce(outcomes);
        FinishCombatIfNeeded();
    }

    private void EndRound()
    {
        ApplyRoundEndPassives();
        StopDefending();
        GrantBoostPoints();
    }

    private void ApplyRoundEndPassives()
    {
        foreach (Traveler traveler in _travelers.Where(traveler => traveler.IsAlive))
            foreach (PassiveSkill passive in traveler.PassiveSkills)
                passive.OnRoundEnd(traveler);
    }

    private void StopDefending()
    {
        foreach (Unit unit in _travelers.Cast<Unit>().Concat(_beasts))
            unit.StopDefending();
    }

    private void GrantBoostPoints()
    {
        foreach (Traveler traveler in _travelers.Where(traveler => traveler.IsAlive))
            traveler.GrantBoostPoint();
    }

    private void Announce(IReadOnlyList<SkillOutcome> outcomes)
    {
        ApplyPriorityChanges(outcomes);
        _reporter.AnnounceOutcomes(outcomes);
    }

    private void ApplyPriorityChanges(IReadOnlyList<SkillOutcome> outcomes)
    {
        foreach (PriorityOutcome priority in outcomes.OfType<PriorityOutcome>())
            MoveToEndOfCurrentRound(priority);
    }

    private void MoveToEndOfCurrentRound(PriorityOutcome outcome)
    {
        int index = _currentRoundOrder.IndexOf(outcome.Target);
        if (outcome.Increased || index < _currentRoundIndex)
            return;

        _currentRoundOrder.RemoveAt(index);
        _currentRoundOrder.Add(outcome.Target);
    }
}
