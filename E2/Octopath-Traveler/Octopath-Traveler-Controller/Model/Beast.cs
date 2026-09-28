namespace Octopath_Traveler;

public class Beast : Unit
{
    public BeastSkill Skill { get; }
    public int Shields { get; }
    public IReadOnlyList<string> Weaknesses { get; }

    public Beast(string name, Stats stats, BeastSkill skill, int shields,
        IReadOnlyList<string> weaknesses)
        : base(name, stats)
    {
        Skill = skill;
        Shields = shields;
        Weaknesses = weaknesses;
    }
}
