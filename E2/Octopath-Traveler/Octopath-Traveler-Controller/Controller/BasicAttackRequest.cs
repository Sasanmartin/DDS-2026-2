namespace Octopath_Traveler;

public class BasicAttackRequest
{
    public required string Weapon { get; init; }
    public required Unit Target { get; init; }
    public int BoostPoints { get; init; }
}
