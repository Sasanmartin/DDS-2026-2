namespace Octopath_Traveler;

public class DamageProfile
{
    public IReadOnlyList<AttackType> Hits { get; init; } = Array.Empty<AttackType>();
    public double Modifier { get; init; }
    public IDamageBonus DamageBonus { get; init; } = new NoDamageBonus();
    public IDamageCap DamageCap { get; init; } = new NoDamageCap();

    public double ApplyBonus(double baseDamage, Unit user)
        => DamageBonus.Apply(baseDamage, user);

    public int ApplyCap(int damage, Unit target)
        => DamageCap.Apply(damage, target);
}

public class DamageRequest
{
    public DamageRequest(Unit attacker, Unit target, AttackType type, DamageProfile profile)
    {
        Attacker = attacker;
        Target = target;
        Type = type;
        Profile = profile;
    }

    public Unit Attacker { get; }
    public Unit Target { get; }
    public AttackType Type { get; }
    public DamageProfile Profile { get; }
}
