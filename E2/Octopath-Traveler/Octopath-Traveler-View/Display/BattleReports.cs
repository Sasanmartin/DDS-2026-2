namespace Octopath_Traveler_View;

public enum DamageKind
{
    Typed,
    Physical,
    Elemental,
    Untyped
}

public record DamageReport(string TargetName, int Damage, DamageKind Kind, string TypeName,
    bool Weakness, bool TargetDefending, bool EnteredBreakingPoint);

public record HealReport(string TargetName, int Amount);

public record ReviveReport(string TargetName);

public record PriorityReport(string TargetName, int Rounds);

public record HpReport(string Name, int CurrentHp);
