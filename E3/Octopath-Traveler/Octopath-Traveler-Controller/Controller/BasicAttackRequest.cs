namespace Octopath_Traveler;

public class BasicAttackRequest
{
    public BasicAttackRequest(string weapon, Unit target, int boostPoints)
    {
        Weapon = weapon;
        Target = target;
        BoostPoints = boostPoints;
    }

    public string Weapon { get; }
    public Unit Target { get; }
    public int BoostPoints { get; }
}
