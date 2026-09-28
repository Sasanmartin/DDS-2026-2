using Octopath_Traveler_View;

namespace Octopath_Traveler;

public class Game
{
    private const string ChooseFileMessage = "Elige un archivo para cargar los equipos";
    private const string InvalidTeamMessage = "Archivo de equipos no válido";

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
        string[] teamFiles = GetTeamFilesInOrder();
        AnnounceAvailableTeamFiles(teamFiles);
        string selectedFile = AskUserToSelectTeamFile(teamFiles);

        TeamDefinition team = new TeamLoader().Parse(selectedFile);
        if (!new TeamValidator().IsValid(team))
        {
            _view.WriteLine(InvalidTeamMessage);
            return;
        }

        (List<Traveler> travelers, List<Beast> beasts) = BuildTeams(team);
        new Battle(_view, travelers, beasts).Run();
    }

    private (List<Traveler> travelers, List<Beast> beasts) BuildTeams(TeamDefinition team)
    {
        DataLoader dataLoader = new(_dataDirectory);
        TeamBuilder teamBuilder = new(dataLoader.LoadCharacters(), dataLoader.LoadEnemies(), dataLoader.LoadBeastSkills());
        return (teamBuilder.BuildTravelers(team), teamBuilder.BuildBeasts(team));
    }

    private string[] GetTeamFilesInOrder()
        => Directory.GetFiles(_teamsFolder, "*.txt").OrderBy(file => file).ToArray();

    private void AnnounceAvailableTeamFiles(string[] teamFiles)
    {
        _view.WriteLine(ChooseFileMessage);
        for (int i = 0; i < teamFiles.Length; i++)
            _view.WriteLine($"{i}: {Path.GetFileName(teamFiles[i])}");
    }

    private string AskUserToSelectTeamFile(string[] teamFiles)
    {
        int selectedIndex = int.Parse(_view.ReadLine());
        return teamFiles[selectedIndex];
    }
}
