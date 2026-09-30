namespace Octopath_Traveler;

public class BasicAttackAction : ITravelerAction
{
    public TurnOutcome Execute(CombatController controller, Traveler traveler)
    {
        BasicAttackRequest? request = controller.SelectBasicAttack(traveler);
        if (request is null)
            return TurnOutcome.Cancelled;

        controller.ResolveBasicAttack(traveler, request);
        return TurnOutcome.Completed;
    }
}
