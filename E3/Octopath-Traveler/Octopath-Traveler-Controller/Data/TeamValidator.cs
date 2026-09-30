namespace Octopath_Traveler;

public class TeamValidator
{
    private const int MinTeamSize = 1;
    private const int MaxTravelers = 4;
    private const int MaxBeasts = 5;
    private const int MaxActiveSkills = 8;
    private const int MaxPassiveSkills = 4;

    public bool IsValid(TeamDefinition team)
    {
        if (team.Travelers.Count < MinTeamSize || team.Travelers.Count > MaxTravelers)
            return false;
        if (team.BeastNames.Count < MinTeamSize || team.BeastNames.Count > MaxBeasts)
            return false;
        if (HasDuplicates(team.Travelers.Select(traveler => traveler.Name)))
            return false;
        if (HasDuplicates(team.BeastNames))
            return false;

        return team.Travelers.All(IsTravelerValid);
    }

    private bool IsTravelerValid(TravelerDefinition traveler)
    {
        if (traveler.ActiveSkills.Count > MaxActiveSkills)
            return false;
        if (traveler.PassiveSkills.Count > MaxPassiveSkills)
            return false;

        return !HasDuplicates(traveler.ActiveSkills) && !HasDuplicates(traveler.PassiveSkills);
    }

    private bool HasDuplicates(IEnumerable<string> names)
    {
        HashSet<string> seenNames = new();
        foreach (string name in names)
            if (!seenNames.Add(name))
                return true;
        return false;
    }
}
