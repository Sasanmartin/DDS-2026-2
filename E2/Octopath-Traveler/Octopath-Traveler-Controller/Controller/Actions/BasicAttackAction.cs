namespace Octopath_Traveler;

public class BasicAttackAction : ITravelerAction
{
    public TurnOutcome Execute(ActionSelector selector, CombatController controller, Traveler traveler)
    {
        BasicAttackRequest? request = selector.SelectBasicAttack(traveler);
        if (request is null)
            return TurnOutcome.Cancelled;

        controller.ResolveBasicAttack(traveler, request);
        return TurnOutcome.Completed;
    }
}
