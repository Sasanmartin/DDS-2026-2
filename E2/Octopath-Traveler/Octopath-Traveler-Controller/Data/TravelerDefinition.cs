namespace Octopath_Traveler;

public class TravelerDefinition
{
    public string Name { get; }
    public List<string> ActiveSkills { get; }
    public List<string> PassiveSkills { get; }

    public TravelerDefinition(string name, List<string> activeSkills, List<string> passiveSkills)
    {
        Name = name;
        ActiveSkills = activeSkills;
        PassiveSkills = passiveSkills;
    }
}
