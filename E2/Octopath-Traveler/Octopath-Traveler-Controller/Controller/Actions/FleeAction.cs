namespace Octopath_Traveler;

public class FleeAction : ITravelerAction
{
    public TurnOutcome Execute(ActionSelector selector, CombatController controller, Traveler traveler)
    {
        controller.Flee();
        return TurnOutcome.Completed;
    }
}
