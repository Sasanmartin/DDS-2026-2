using Octopath_Traveler_View;

namespace Octopath_Traveler;

public class Game
{
    private readonly View _view;
    private readonly string _teamsFolder;
    private readonly string _dataDirectory;

    public Game(View view, string teamsFolder)
    {
        _view = view;
        _teamsFolder = teamsFolder;
        _dataDirectory = Path.GetDirectoryName(teamsFolder) ?? ".";
    }

    public void Play()
    {
        TeamDefinition? team = LoadSelectedTeam();
        if (team is null)
            return;

        (List<Traveler> travelers, List<Beast> beasts) = BuildTeams(team);
        new CombatController(_view, travelers, beasts).Run();
    }

    private TeamDefinition? LoadSelectedTeam()
    {
        string[] teamFiles = GetTeamFilesInOrder();
        int selectedIndex = _view.SelectTeamFile(GetFileNames(teamFiles));
        TeamDefinition team = new TeamLoader().Parse(teamFiles[selectedIndex]);

        if (new TeamValidator().IsValid(team))
            return team;

        _view.AnnounceInvalidTeam();
        return null;
    }

    private static List<string> GetFileNames(string[] teamFiles)
        => teamFiles.Select(file => Path.GetFileName(file) ?? file).ToList();

    private (List<Traveler> travelers, List<Beast> beasts) BuildTeams(TeamDefinition team)
    {
        DataLoader dataLoader = new(_dataDirectory);
        TeamBuilder teamBuilder = new(BuildGameData(dataLoader));
        return (teamBuilder.BuildTravelers(team), teamBuilder.BuildBeasts(team));
    }

    private static GameData BuildGameData(DataLoader dataLoader)
    {
        DamageCalculator calculator = new();
        return new GameData
        {
            Travelers = dataLoader.LoadTravelers(),
            Beasts = dataLoader.LoadBeasts(),
            BeastSkills = dataLoader.LoadBeastSkills(),
            ActiveSkills = new ActiveSkillFactory(dataLoader.LoadActiveSkills(), calculator),
            PassiveSkills = new PassiveSkillFactory()
        };
    }

    private string[] GetTeamFilesInOrder()
        => Directory.GetFiles(_teamsFolder, "*.txt").OrderBy(file => file).ToArray();
}
