namespace Octopath_Traveler;

public class GameData
{
    public required Dictionary<string, TravelerData> Travelers { get; init; }
    public required Dictionary<string, BeastData> Beasts { get; init; }
    public required Dictionary<string, BeastSkill> BeastSkills { get; init; }
    public required ActiveSkillFactory ActiveSkills { get; init; }
    public required PassiveSkillFactory PassiveSkills { get; init; }
}
