namespace Octopath_Traveler_View;

public record TurnQueuesDisplay(IReadOnlyList<string> CurrentRoundNames,
    IReadOnlyList<string> NextRoundNames);
