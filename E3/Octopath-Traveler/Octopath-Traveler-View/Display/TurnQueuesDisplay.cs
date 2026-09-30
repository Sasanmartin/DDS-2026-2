namespace Octopath_Traveler_View;

public class TurnQueuesDisplay
{
    public IReadOnlyList<string> CurrentRoundNames { get; set; } = Array.Empty<string>();
    public IReadOnlyList<string> NextRoundNames { get; set; } = Array.Empty<string>();
}
