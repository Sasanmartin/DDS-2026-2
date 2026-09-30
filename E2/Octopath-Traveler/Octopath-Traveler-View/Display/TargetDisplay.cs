namespace Octopath_Traveler_View;

public record TargetDisplay(string Name);

public sealed record TravelerTargetDisplay(string Name, int CurrentHp, int MaxHp,
    int CurrentSp, int MaxSp, int BoostPoints) : TargetDisplay(Name);

public sealed record BeastTargetDisplay(string Name, int CurrentHp, int MaxHp, int Shields)
    : TargetDisplay(Name);
