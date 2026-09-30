namespace Octopath_Traveler;

public class FleeAction : ITravelerAction
{
    public TurnOutcome Execute(CombatController controller, Traveler traveler)
    {
        controller.Flee();
        return TurnOutcome.Completed;
    }
}
