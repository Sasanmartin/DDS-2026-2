namespace Octopath_Traveler;

public class UseSkillAction : ITravelerAction
{
    public TurnOutcome Execute(ActionSelector selector, CombatController controller, Traveler traveler)
    {
        SkillRequest? request = selector.SelectSkillRequest(traveler);
        if (request is null)
            return TurnOutcome.Cancelled;

        controller.ResolveSkill(traveler, request);
        return TurnOutcome.Completed;
    }
}
