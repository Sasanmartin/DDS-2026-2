namespace Octopath_Traveler;

public class BasicAttackRequest
{
    public BasicAttackRequest(string weapon, Unit target)
    {
        Weapon = weapon;
        Target = target;
    }

    public string Weapon { get; }
    public Unit Target { get; }
}
