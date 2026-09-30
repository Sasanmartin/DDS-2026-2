namespace Octopath_Traveler_View;

public enum DamageKind
{
    Typed,
    Physical,
    Elemental,
    Untyped
}

public class DamageReport
{
    public string TargetName { get; set; } = "";
    public int Damage { get; set; }
    public DamageKind Kind { get; set; }
    public string TypeName { get; set; } = "";
    public bool Weakness { get; set; }
    public bool TargetDefending { get; set; }
    public bool EnteredBreakingPoint { get; set; }
}

public class HealReport
{
    public string TargetName { get; set; } = "";
    public int Amount { get; set; }
}

public class ReviveReport
{
    public string TargetName { get; set; } = "";
}

public class PriorityReport
{
    public string TargetName { get; set; } = "";
    public int Rounds { get; set; }
}

public class HpReport
{
    public string Name { get; set; } = "";
    public int CurrentHp { get; set; }
}
