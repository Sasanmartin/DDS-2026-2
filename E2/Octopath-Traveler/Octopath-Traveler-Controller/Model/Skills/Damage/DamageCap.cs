namespace Octopath_Traveler;

public interface IDamageCap
{
    int Apply(int damage, Unit target);
}

public class NoDamageCap : IDamageCap
{
    public int Apply(int damage, Unit target) => damage;
}

public class LeaveAtLeastOneHpCap : IDamageCap
{
    private const int MinimumHp = 1;

    public int Apply(int damage, Unit target)
        => Math.Min(damage, Math.Max(0, target.CurrentHp - MinimumHp));
}
