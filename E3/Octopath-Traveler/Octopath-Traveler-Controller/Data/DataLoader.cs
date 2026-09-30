using System.Text.Json;

namespace Octopath_Traveler;

public class DataLoader
{
    private const string TravelersFileName = "characters.json";
    private const string BeastsFileName = "enemies.json";
    private const string BeastSkillsFileName = "beast_skills.json";
    private const string ActiveSkillsFileName = "skills.json";
    private const string PassiveSkillsFileName = "passive_skills.json";

    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    private readonly string _dataDirectory;

    public DataLoader(string dataDirectory)
    {
        _dataDirectory = dataDirectory;
    }

    public Dictionary<string, TravelerData> LoadTravelers()
        => LoadCatalog<TravelerData>(TravelersFileName, traveler => traveler.Name);

    public Dictionary<string, BeastData> LoadBeasts()
        => LoadCatalog<BeastData>(BeastsFileName, beast => beast.Name);

    public Dictionary<string, BeastSkill> LoadBeastSkills()
        => LoadCatalog<BeastSkill>(BeastSkillsFileName, skill => skill.Name);

    public Dictionary<string, ActiveSkillData> LoadActiveSkills()
        => LoadCatalog<ActiveSkillData>(ActiveSkillsFileName, skill => skill.Name);

    public Dictionary<string, PassiveSkillData> LoadPassiveSkills()
        => LoadCatalog<PassiveSkillData>(PassiveSkillsFileName, skill => skill.Name);

    private Dictionary<string, T> LoadCatalog<T>(string fileName, Func<T, string> keySelector)
    {
        string filePath = Path.Combine(_dataDirectory, fileName);
        string json = File.ReadAllText(filePath);
        List<T> items = JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? new List<T>();
        return items.ToDictionary(keySelector);
    }
}
