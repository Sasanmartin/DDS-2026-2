namespace Octopath_Traveler;

public class TeamBuilder
{
    private readonly GameData _data;

    public TeamBuilder(GameData data)
    {
        _data = data;
    }

    public List<Traveler> BuildTravelers(TeamDefinition team)
        => team.Travelers.Select(BuildTraveler).ToList();

    public List<Beast> BuildBeasts(TeamDefinition team)
        => team.BeastNames.Select(BuildBeast).ToList();

    private Traveler BuildTraveler(TravelerDefinition definition)
    {
        TravelerData travelerData = _data.Travelers[definition.Name];
        List<PassiveSkill> passives = definition.PassiveSkills
            .Select(_data.PassiveSkills.Create)
            .ToList();
        Stats stats = ApplyPassives(travelerData.Stats, passives);
        List<ActiveSkill> activeSkills = definition.ActiveSkills
            .Select(_data.ActiveSkills.Create)
            .ToList();
        return new Traveler(definition.Name, stats, travelerData.Weapons, activeSkills, passives);
    }

    private static Stats ApplyPassives(Stats baseStats, IEnumerable<PassiveSkill> passives)
    {
        Stats stats = baseStats.Copy();
        foreach (PassiveSkill passive in passives)
            passive.ApplyTo(stats);
        return stats;
    }

    private Beast BuildBeast(string name)
    {
        BeastData data = _data.Beasts[name];
        BeastSkill skill = _data.BeastSkills[data.Skill];
        return new Beast(name, data.Stats, skill, data.Shields, data.Weaknesses);
    }
}
