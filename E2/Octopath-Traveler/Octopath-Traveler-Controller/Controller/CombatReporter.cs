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
        _view.ShowTurnQueues(new TurnQueuesDisplay
        {
            CurrentRoundNames = currentRound.Select(unit => unit.Name).ToList(),
            NextRoundNames = nextRound.Select(unit => unit.Name).ToList()
        });
    }

    public IReadOnlyList<TargetDisplay> ToTargetDisplays(IReadOnlyList<Unit> units)
        => units.Select(ToTargetDisplay).ToList();

    public void AnnounceBasicAttack(string travelerName)
        => _view.AnnounceBasicAttack(travelerName);

    public void AnnounceSkillUse(string actorName, string skillName)
        => _view.AnnounceSkillUse(actorName, skillName);

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
            case HealOutcome heal: _view.AnnounceHeal(BuildHeal(heal)); break;
            case ReviveOutcome revive: _view.AnnounceRevive(new ReviveReport { TargetName = revive.Target.Name }); break;
            case PriorityOutcome priority: AnnouncePriority(priority); break;
        }
    }

    private static HealReport BuildHeal(HealOutcome outcome)
        => new()
        {
            TargetName = outcome.Target.Name,
            Amount = outcome.Amount
        };

    private void AnnouncePriority(PriorityOutcome outcome)
    {
        if (!outcome.Increased)
            _view.AnnouncePriority(new PriorityReport
            {
                TargetName = outcome.Target.Name,
                Rounds = outcome.Rounds
            });
    }

    private BoardDisplay BuildBoard()
        => new()
        {
            Travelers = _travelers.Select((traveler, index) => new TravelerDisplay
            {
                Letter = LetterFor(index),
                Name = traveler.Name,
                CurrentHp = traveler.CurrentHp,
                MaxHp = traveler.MaxHp,
                CurrentSp = traveler.CurrentSp,
                MaxSp = traveler.MaxSp,
                BoostPoints = traveler.BoostPoints
            }).ToList(),
            Beasts = _beasts.Select((beast, index) => new BeastDisplay
            {
                Letter = LetterFor(index),
                Name = beast.Name,
                CurrentHp = beast.CurrentHp,
                MaxHp = beast.MaxHp,
                Shields = beast.Shields
            }).ToList()
        };

    private static IReadOnlyList<HpReport> BuildHpResults(IReadOnlyList<SkillOutcome> outcomes)
        => AffectedUnits(outcomes)
            .Select(unit => new HpReport { Name = unit.Name, CurrentHp = unit.CurrentHp })
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
        => new()
        {
            TargetName = outcome.Target.Name,
            Damage = outcome.Damage,
            Kind = DamageKindOf(outcome),
            TypeName = outcome.Type.Name,
            Weakness = outcome.Weakness,
            TargetDefending = outcome.Target.IsDefending,
            EnteredBreakingPoint = outcome.EnteredBreakingPoint
        };

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
            Traveler traveler => new TravelerTargetDisplay
            {
                Name = traveler.Name,
                CurrentHp = traveler.CurrentHp,
                MaxHp = traveler.MaxHp,
                CurrentSp = traveler.CurrentSp,
                MaxSp = traveler.MaxSp,
                BoostPoints = traveler.BoostPoints
            },
            Beast beast => new BeastTargetDisplay
            {
                Name = beast.Name,
                CurrentHp = beast.CurrentHp,
                MaxHp = beast.MaxHp,
                Shields = beast.Shields
            },
            _ => new TargetDisplay { Name = unit.Name }
        };

    private static string LetterFor(int index)
        => ((char)('A' + index)).ToString();
}
