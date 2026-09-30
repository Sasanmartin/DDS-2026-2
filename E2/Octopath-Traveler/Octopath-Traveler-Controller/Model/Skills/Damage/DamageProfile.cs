namespace Octopath_Traveler;

public class DamageProfile
{
    public IReadOnlyList<AttackType> Hits { get; init; } = Array.Empty<AttackType>();
    public double Modifier { get; init; }
    public IDamageScaling Scaling { get; init; } = new FixedDamageScaling();
    public IDamageLimit Limit { get; init; } = new FullDamageLimit();

    public double ApplyScaling(double baseDamage, Unit user)
        => Scaling.Apply(baseDamage, user);

    public int ApplyLimit(int damage, Unit target)
        => Limit.Limit(damage, target);
}

public class DamageRequest
{
    public required Unit Attacker { get; init; }
    public required Unit Target { get; init; }
    public required AttackType Type { get; init; }
    public required DamageProfile Profile { get; init; }
}
