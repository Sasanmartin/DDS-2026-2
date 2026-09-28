namespace Octopath_Traveler;

public class Traveler : Unit
{
    private const int MaxBoostPoints = 5;
    private const int InitialBoostPoints = 1;

    public int CurrentSp { get; private set; }
    public int BoostPoints { get; private set; }
    public IReadOnlyList<string> Weapons { get; }
    public IReadOnlyList<string> ActiveSkills { get; }
    public IReadOnlyList<string> PassiveSkills { get; }

    public int MaxSp => Stats.SP;

    public Traveler(string name, Stats stats, IReadOnlyList<string> weapons,
        IReadOnlyList<string> activeSkills, IReadOnlyList<string> passiveSkills)
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
        if (BoostPoints < MaxBoostPoints)
            BoostPoints++;
    }
}
