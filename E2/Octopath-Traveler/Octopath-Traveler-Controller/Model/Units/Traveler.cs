namespace Octopath_Traveler;

public class Traveler : Unit
{
    private const int MaxBoostPoints = 5;
    private const int InitialBoostPoints = 1;

    public int CurrentSp { get; private set; }
    public int BoostPoints { get; private set; }
    public bool HasBoostedThisRound { get; private set; }
    public IReadOnlyList<string> Weapons { get; }
    public IReadOnlyList<ActiveSkill> ActiveSkills { get; }
    public IReadOnlyList<PassiveSkill> PassiveSkills { get; }

    public int MaxSp => Stats.SP;

    public Traveler(string name, Stats stats, IReadOnlyList<string> weapons,
        IReadOnlyList<ActiveSkill> activeSkills, IReadOnlyList<PassiveSkill> passiveSkills)
        : base(name, stats)
    {
        CurrentSp = stats.SP;
        BoostPoints = InitialBoostPoints;
        Weapons = weapons;
        ActiveSkills = activeSkills;
        PassiveSkills = passiveSkills;
    }

    public void GrantBoostPoint()
    {
        if (!HasBoostedThisRound)
            GrantExtraBoostPoint();
        HasBoostedThisRound = false;
    }

    public void GrantExtraBoostPoint()
        => BoostPoints = Math.Min(MaxBoostPoints, BoostPoints + 1);

    public void ConsumeBoostPoints(int amount)
    {
        if (amount <= 0)
            return;
        BoostPoints = Math.Max(0, BoostPoints - amount);
        HasBoostedThisRound = true;
    }

    public void RecoverSp(int amount)
        => CurrentSp = Math.Min(MaxSp, CurrentSp + amount);

    public bool HasEnoughSp(int amount)
        => CurrentSp >= amount;

    public void SpendSp(int amount)
        => CurrentSp = Math.Max(0, CurrentSp - amount);
}
