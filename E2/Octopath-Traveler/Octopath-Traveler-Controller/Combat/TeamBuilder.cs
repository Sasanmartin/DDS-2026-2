namespace Octopath_Traveler;

public class TeamBuilder
{
    private readonly Dictionary<string, CharacterData> _characters;
    private readonly Dictionary<string, EnemyData> _enemies;
    private readonly Dictionary<string, BeastSkill> _beastSkills;

    public TeamBuilder(Dictionary<string, CharacterData> characters,
        Dictionary<string, EnemyData> enemies, Dictionary<string, BeastSkill> beastSkills)
    {
        _characters = characters;
        _enemies = enemies;
        _beastSkills = beastSkills;
    }

    public List<Traveler> BuildTravelers(TeamDefinition team)
        => team.Travelers.Select(BuildTraveler).ToList();

    public List<Beast> BuildBeasts(TeamDefinition team)
        => team.BeastNames.Select(BuildBeast).ToList();

    private Traveler BuildTraveler(TravelerDefinition definition)
    {
        CharacterData data = _characters[definition.Name];
        return new Traveler(definition.Name, data.Stats, data.Weapons,
            definition.ActiveSkills, definition.PassiveSkills);
    }

    private Beast BuildBeast(string name)
    {
        EnemyData data = _enemies[name];
        BeastSkill skill = _beastSkills[data.Skill];
        return new Beast(name, data.Stats, skill, data.Shields, data.Weaknesses);
    }
}
