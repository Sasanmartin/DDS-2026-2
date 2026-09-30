namespace Octopath_Traveler_View;

public class TravelerDisplay
{
    public string Letter { get; set; } = "";
    public string Name { get; set; } = "";
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public int CurrentSp { get; set; }
    public int MaxSp { get; set; }
    public int BoostPoints { get; set; }
}

public class BeastDisplay
{
    public string Letter { get; set; } = "";
    public string Name { get; set; } = "";
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public int Shields { get; set; }
}

public class BoardDisplay
{
    public IReadOnlyList<TravelerDisplay> Travelers { get; set; } = Array.Empty<TravelerDisplay>();
    public IReadOnlyList<BeastDisplay> Beasts { get; set; } = Array.Empty<BeastDisplay>();
}
