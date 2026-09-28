namespace Octopath_Traveler;

public class TeamLoader
{
    private const string PlayerTeamHeader = "Player Team";
    private const string EnemyTeamHeader = "Enemy Team";

    public TeamDefinition Parse(string filePath)
    {
        TeamDefinition team = new();
        bool isReadingEnemies = false;

        foreach (string line in File.ReadLines(filePath))
        {
            if (line == PlayerTeamHeader)
                continue;
            if (line == EnemyTeamHeader)
            {
                isReadingEnemies = true;
                continue;
            }
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (isReadingEnemies)
                team.BeastNames.Add(line.Trim());
            else
                team.Travelers.Add(ParseTraveler(line.Trim()));
        }

        return team;
    }

    private TravelerDefinition ParseTraveler(string line)
    {
        string name = ExtractName(line);
        List<string> activeSkills = ExtractSkills(line, '(', ')');
        List<string> passiveSkills = ExtractSkills(line, '[', ']');
        return new TravelerDefinition(name, activeSkills, passiveSkills);
    }

    private string ExtractName(string line)
    {
        int delimiterIndex = FindFirstDelimiter(line);
        return delimiterIndex == -1 ? line : line[..delimiterIndex].Trim();
    }

    private int FindFirstDelimiter(string line)
    {
        int openParenthesis = line.IndexOf('(');
        int openBracket = line.IndexOf('[');

        if (openParenthesis == -1)
            return openBracket;
        if (openBracket == -1)
            return openParenthesis;
        return Math.Min(openParenthesis, openBracket);
    }

    private List<string> ExtractSkills(string line, char opening, char closing)
    {
        int openingIndex = line.IndexOf(opening);
        int closingIndex = line.IndexOf(closing);

        if (openingIndex == -1 || closingIndex == -1 || closingIndex < openingIndex)
            return new List<string>();

        string content = line[(openingIndex + 1)..closingIndex];
        if (string.IsNullOrWhiteSpace(content))
            return new List<string>();

        return content.Split(',').Select(skill => skill.Trim()).ToList();
    }
}
