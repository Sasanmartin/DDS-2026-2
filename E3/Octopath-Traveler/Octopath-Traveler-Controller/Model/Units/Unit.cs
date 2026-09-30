namespace Octopath_Traveler;

public abstract class Unit
{
    private const int ReviveHp = 1;

    public string Name { get; }
    public Stats Stats { get; }
    public int CurrentHp { get; private set; }

    private UnitStatus Status { get; } = new();

    public int MaxHp => Stats.HP;
    public bool IsAlive => CurrentHp > 0;
    public bool IsDefending => Status.IsDefending;

    protected Unit(string name, Stats stats)
    {
        Name = name;
        Stats = stats;
        CurrentHp = stats.HP;
    }

    public void TakeDamage(int damage)
        => CurrentHp = Math.Max(0, CurrentHp - damage);

    public void Heal(int amount)
    {
        if (IsAlive)
            CurrentHp = Math.Min(MaxHp, CurrentHp + amount);
    }

    public void Revive()
    {
        if (!IsAlive)
            CurrentHp = ReviveHp;
    }

    public void BeginDefending(int round)
        => Status.BeginDefending(round);

    public void StopDefending()
        => Status.StopDefending();

    public void IncreasePriorityForRound(int round)
        => Status.IncreasePriorityForRound(round);

    public void DecreasePriorityFrom(int round)
        => Status.DecreasePriorityFrom(round);

    public bool DefendedInRound(int round)
        => Status.DefendedInRound(round);

    public bool HasIncreasedPriorityIn(int round)
        => Status.HasIncreasedPriorityIn(round);

    public bool HasDecreasedPriorityIn(int round)
        => Status.HasDecreasedPriorityIn(round);
}
