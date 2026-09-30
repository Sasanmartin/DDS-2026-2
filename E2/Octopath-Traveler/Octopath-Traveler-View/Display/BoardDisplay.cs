namespace Octopath_Traveler_View;

public record TravelerDisplay(string Letter, string Name, int CurrentHp, int MaxHp,
    int CurrentSp, int MaxSp, int BoostPoints);

public record BeastDisplay(string Letter, string Name, int CurrentHp, int MaxHp, int Shields);

public record BoardDisplay(IReadOnlyList<TravelerDisplay> Travelers, IReadOnlyList<BeastDisplay> Beasts);
