namespace Octopath_Traveler;

public abstract class Unit
{
    public string Name { get; }
    public Stats Stats { get; }
    public int CurrentHp { get; private set; }

    public int MaxHp => Stats.HP;
    public bool IsAlive => CurrentHp > 0;

    protected Unit(string name, Stats stats)
    {
        Name = name;
        Stats = stats;
        CurrentHp = stats.HP;
    }

    public void TakeDamage(int damage)
        => CurrentHp = Math.Max(0, CurrentHp - damage);
}
