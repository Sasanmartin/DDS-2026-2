namespace Octopath_Traveler;

public abstract class SkillOutcome
{
}

public class DamageOutcome : SkillOutcome
{
    public DamageOutcome(Unit attacker, Unit target, int damage, AttackType type,
        bool weakness, bool enteredBreakingPoint)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
        Type = type;
        Weakness = weakness;
        EnteredBreakingPoint = enteredBreakingPoint;
    }

    public Unit Attacker { get; }
    public Unit Target { get; }
    public int Damage { get; }
    public AttackType Type { get; }
    public bool Weakness { get; }
    public bool EnteredBreakingPoint { get; }
}

public class HealOutcome : SkillOutcome
{
    public HealOutcome(Unit target, int amount)
    {
        Target = target;
        Amount = amount;
    }

    public Unit Target { get; }
    public int Amount { get; }
}

public class ReviveOutcome : SkillOutcome
{
    public ReviveOutcome(Unit target)
        => Target = target;

    public Unit Target { get; }
}

public class PriorityOutcome : SkillOutcome
{
    public PriorityOutcome(Unit target, int rounds, bool increased)
    {
        Target = target;
        Rounds = rounds;
        Increased = increased;
    }

    public Unit Target { get; }
    public int Rounds { get; }
    public bool Increased { get; }
}
