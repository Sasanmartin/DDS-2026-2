using System.Text.Json;

namespace Octopath_Traveler;

public class DataLoader
{
    private const string CharactersFileName = "characters.json";
    private const string EnemiesFileName = "enemies.json";
    private const string BeastSkillsFileName = "beast_skills.json";

    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    private readonly string _dataDirectory;

    public DataLoader(string dataDirectory)
    {
        _dataDirectory = dataDirectory;
    }

    public Dictionary<string, CharacterData> LoadCharacters()
        => LoadCatalog<CharacterData>(CharactersFileName, character => character.Name);

    public Dictionary<string, EnemyData> LoadEnemies()
        => LoadCatalog<EnemyData>(EnemiesFileName, enemy => enemy.Name);

    public Dictionary<string, BeastSkill> LoadBeastSkills()
        => LoadCatalog<BeastSkill>(BeastSkillsFileName, skill => skill.Name);

    private Dictionary<string, T> LoadCatalog<T>(string fileName, Func<T, string> keySelector)
    {
        string filePath = Path.Combine(_dataDirectory, fileName);
        string json = File.ReadAllText(filePath);
        List<T> items = JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? new List<T>();
        return items.ToDictionary(keySelector);
    }
}
