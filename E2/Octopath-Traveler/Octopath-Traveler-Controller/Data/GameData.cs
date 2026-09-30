namespace Octopath_Traveler;

public class GameData
{
    public GameData(Dictionary<string, TravelerData> travelers,
        Dictionary<string, BeastData> beasts,
        Dictionary<string, BeastSkill> beastSkills,
        ActiveSkillFactory activeSkills,
        PassiveSkillFactory passiveSkills)
    {
        Travelers = travelers;
        Beasts = beasts;
        BeastSkills = beastSkills;
        ActiveSkills = activeSkills;
        PassiveSkills = passiveSkills;
    }

    public Dictionary<string, TravelerData> Travelers { get; }
    public Dictionary<string, BeastData> Beasts { get; }
    public Dictionary<string, BeastSkill> BeastSkills { get; }
    public ActiveSkillFactory ActiveSkills { get; }
    public PassiveSkillFactory PassiveSkills { get; }
}
