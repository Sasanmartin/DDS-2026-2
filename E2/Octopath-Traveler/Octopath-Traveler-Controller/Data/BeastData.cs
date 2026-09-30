namespace Octopath_Traveler;

public class BeastData
{
    public string Name { get; set; } = "";
    public Stats Stats { get; set; } = new();
    public string Skill { get; set; } = "";
    public int Shields { get; set; }
    public List<string> Weaknesses { get; set; } = new();
}
