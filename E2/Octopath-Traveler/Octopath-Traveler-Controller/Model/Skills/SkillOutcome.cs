namespace Octopath_Traveler;

public abstract record SkillOutcome;

public sealed record DamageOutcome(
    Unit Attacker,
    Unit Target,
    int Damage,
    AttackType Type,
    bool Weakness,
    bool EnteredBreakingPoint) : SkillOutcome;

public sealed record HealOutcome(Unit Target, int Amount) : SkillOutcome;

public sealed record ReviveOutcome(Unit Target) : SkillOutcome;

public sealed record PriorityOutcome(Unit Target, int Rounds, bool Increased) : SkillOutcome;
