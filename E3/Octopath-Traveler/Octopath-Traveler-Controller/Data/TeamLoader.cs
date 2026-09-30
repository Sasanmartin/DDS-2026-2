namespace Octopath_Traveler;

public class TeamLoader
{
    // Formato del archivo de equipos:
    //   Player Team
    //   Viajero (Habilidad activa, ...) [Habilidad pasiva, ...]
    //   Enemy Team
    //   Bestia
    private const string PlayerTeamHeader = "Player Team";
    private const string EnemyTeamHeader = "Enemy Team";
    private const char ActiveSkillOpening = '(';
    private const char ActiveSkillClosing = ')';
    private const char PassiveSkillOpening = '[';
    private const char PassiveSkillClosing = ']';

    public TeamDefinition Parse(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        TeamDefinition team = new();
        team.Travelers.AddRange(ParseTravelers(lines));
        team.BeastNames.AddRange(ParseBeasts(lines));
        return team;
    }

    private static IEnumerable<TravelerDefinition> ParseTravelers(IEnumerable<string> lines)
        => LinesOfSection(lines, PlayerTeamHeader, EnemyTeamHeader)
            .Select(line => ParseTraveler(line.Trim()));

    private static IEnumerable<string> ParseBeasts(IEnumerable<string> lines)
        => LinesOfSection(lines, EnemyTeamHeader, endHeader: null)
            .Select(line => line.Trim());

    private static IEnumerable<string> LinesOfSection(IEnumerable<string> lines, string startHeader,
        string? endHeader)
        => lines
            .SkipWhile(line => line != startHeader)
            .Skip(1)
            .TakeWhile(line => endHeader is null || line != endHeader)
            .Where(HasContent);

    private static bool HasContent(string line)
        => !string.IsNullOrWhiteSpace(line);

    private static TravelerDefinition ParseTraveler(string line)
    {
        string name = ExtractName(line);
        List<string> activeSkills = ExtractSkills(line, ActiveSkillOpening, ActiveSkillClosing);
        List<string> passiveSkills = ExtractSkills(line, PassiveSkillOpening, PassiveSkillClosing);
        return new TravelerDefinition(name, activeSkills, passiveSkills);
    }

    private static string ExtractName(string line)
    {
        int delimiterIndex = FindFirstDelimiter(line);
        return delimiterIndex == -1 ? line : line[..delimiterIndex].Trim();
    }

    private static int FindFirstDelimiter(string line)
    {
        int openParenthesis = line.IndexOf(ActiveSkillOpening);
        int openBracket = line.IndexOf(PassiveSkillOpening);

        if (openParenthesis == -1)
            return openBracket;
        if (openBracket == -1)
            return openParenthesis;
        return Math.Min(openParenthesis, openBracket);
    }

    private static List<string> ExtractSkills(string line, char opening, char closing)
    {
        int openingIndex = line.IndexOf(opening);
        int closingIndex = line.IndexOf(closing);

        if (!HasDelimitedContent(openingIndex, closingIndex))
            return new List<string>();

        string content = line[(openingIndex + 1)..closingIndex];
        return content.Split(',').Select(skill => skill.Trim()).Where(HasContent).ToList();
    }

    private static bool HasDelimitedContent(int openingIndex, int closingIndex)
        => openingIndex != -1 && closingIndex != -1 && closingIndex > openingIndex;
}
