using Octopath_Traveler_View;

namespace Octopath_Traveler;

public class CombatReporter
{
    private readonly View _view;
    private readonly IReadOnlyList<Traveler> _travelers;
    private readonly IReadOnlyList<Beast> _beasts;

    public CombatReporter(View view, CombatState state)
    {
        _view = view;
        _travelers = state.Travelers;
        _beasts = state.Beasts;
    }

    public void AnnounceRoundStart(int round)
        => _view.AnnounceRoundStart(round);

    public void ShowTurnState(IReadOnlyList<Unit> currentRound, IReadOnlyList<Unit> nextRound)
    {
        _view.ShowBoard(BuildBoard());
        _view.ShowTurnQueues(new TurnQueuesDisplay(
            currentRound.Select(unit => unit.Name).ToList(),
            nextRound.Select(unit => unit.Name).ToList()));
    }

    public IReadOnlyList<TargetDisplay> ToTargetDisplays(IReadOnlyList<Unit> units)
        => units.Select(ToTargetDisplay).ToList();

    public void AnnounceBasicAttack(string travelerName)
        => _view.AnnounceBasicAttack(travelerName);

    public void AnnounceSkillUse(string actorName, string skillName)
        => _view.AnnounceSkillUse(actorName, skillName);

    public void AnnounceExtraTurn(string travelerName)
        => _view.AnnounceExtraTurn(travelerName);

    public void AnnounceFlee()
        => _view.AnnounceFlee();

    public void AnnouncePlayerWins()
        => _view.AnnouncePlayerWins();

    public void AnnounceEnemyWins()
        => _view.AnnounceEnemyWins();

    public void AnnounceOutcomes(IReadOnlyList<SkillOutcome> outcomes)
    {
        foreach (SkillOutcome outcome in outcomes)
            AnnounceOutcome(outcome);
        _view.AnnounceHpResults(BuildHpResults(outcomes));
    }

    private void AnnounceOutcome(SkillOutcome outcome)
    {
        switch (outcome)
        {
            case DamageOutcome damage: _view.AnnounceDamage(BuildDamage(damage)); break;
            case HealOutcome heal: _view.AnnounceHeal(new HealReport(heal.Target.Name, heal.Amount)); break;
            case ReviveOutcome revive: _view.AnnounceRevive(new ReviveReport(revive.Target.Name)); break;
            case PriorityOutcome priority: AnnouncePriority(priority); break;
        }
    }

    private void AnnouncePriority(PriorityOutcome outcome)
    {
        if (!outcome.Increased)
            _view.AnnouncePriority(new PriorityReport(outcome.Target.Name, outcome.Rounds));
    }

    private BoardDisplay BuildBoard()
        => new(
            _travelers.Select((traveler, index) => new TravelerDisplay(
                LetterFor(index), traveler.Name, traveler.CurrentHp, traveler.MaxHp,
                traveler.CurrentSp, traveler.MaxSp, traveler.BoostPoints)).ToList(),
            _beasts.Select((beast, index) => new BeastDisplay(
                LetterFor(index), beast.Name, beast.CurrentHp, beast.MaxHp, beast.Shields)).ToList());

    private static IReadOnlyList<HpReport> BuildHpResults(IReadOnlyList<SkillOutcome> outcomes)
        => AffectedUnits(outcomes)
            .Select(unit => new HpReport(unit.Name, unit.CurrentHp))
            .ToList();

    private static IEnumerable<Unit> AffectedUnits(IReadOnlyList<SkillOutcome> outcomes)
        => outcomes.SelectMany(AffectedTargets).Distinct();

    private static IEnumerable<Unit> AffectedTargets(SkillOutcome outcome)
        => outcome switch
        {
            DamageOutcome damage => new[] { damage.Target },
            HealOutcome heal => new[] { heal.Target },
            ReviveOutcome revive => new[] { revive.Target },
            _ => Array.Empty<Unit>()
        };

    private static DamageReport BuildDamage(DamageOutcome outcome)
        => new(
            outcome.Target.Name,
            outcome.Damage,
            DamageKindOf(outcome),
            outcome.Type.Name,
            outcome.Weakness,
            outcome.Target.IsDefending,
            outcome.EnteredBreakingPoint);

    private static DamageKind DamageKindOf(DamageOutcome outcome)
    {
        if (outcome.Type == AttackType.None)
            return DamageKind.Untyped;
        if (outcome.Attacker is Traveler)
            return DamageKind.Typed;
        return outcome.Type.IsElemental ? DamageKind.Elemental : DamageKind.Physical;
    }

    private static TargetDisplay ToTargetDisplay(Unit unit)
        => unit switch
        {
            Traveler traveler => new TravelerTargetDisplay(traveler.Name, traveler.CurrentHp,
                traveler.MaxHp, traveler.CurrentSp, traveler.MaxSp, traveler.BoostPoints),
            Beast beast => new BeastTargetDisplay(beast.Name, beast.CurrentHp, beast.MaxHp, beast.Shields),
            _ => new TargetDisplay(unit.Name)
        };

    private static string LetterFor(int index)
        => ((char)('A' + index)).ToString();
}
