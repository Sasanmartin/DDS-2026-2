namespace Octopath_Traveler_View;

public class TargetDisplay
{
    public string Name { get; set; } = "";
}

public class TravelerTargetDisplay : TargetDisplay
{
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public int CurrentSp { get; set; }
    public int MaxSp { get; set; }
    public int BoostPoints { get; set; }
}

public class BeastTargetDisplay : TargetDisplay
{
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public int Shields { get; set; }
}
