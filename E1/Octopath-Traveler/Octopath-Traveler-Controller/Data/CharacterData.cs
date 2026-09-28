namespace Octopath_Traveler;

public class CharacterData
{
    public string Name { get; set; } = "";
    public Stats Stats { get; set; } = new();
    public List<string> Weapons { get; set; } = new();
}
