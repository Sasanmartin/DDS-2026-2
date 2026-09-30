namespace Octopath_Traveler;

public interface IDamageLimit
{
    int Limit(int damage, Unit target);
}

public class FullDamageLimit : IDamageLimit
{
    public int Limit(int damage, Unit target) => damage;
}

public class LeaveAtLeastOneHpLimit : IDamageLimit
{
    private const int MinimumHp = 1;

    public int Limit(int damage, Unit target)
        => Math.Min(damage, Math.Max(0, target.CurrentHp - MinimumHp));
}
