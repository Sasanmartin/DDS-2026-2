namespace Octopath_Traveler;

public class BeastSkill
{
    private const string ElementalKeyword = "elemental";

    public string Name { get; set; } = "";
    public double Modifier { get; set; }
    public string Description { get; set; } = "";
    public string Target { get; set; } = "";
    public int Hits { get; set; } = 1;

    public bool IsElementalAttack
        => Description.Contains(ElementalKeyword, StringComparison.OrdinalIgnoreCase);
}
