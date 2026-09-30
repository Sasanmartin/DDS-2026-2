namespace Octopath_Traveler;

public class UseSkillAction : ITravelerAction
{
    public TurnOutcome Execute(CombatController controller, Traveler traveler)
    {
        SkillRequest? request = controller.SelectSkillRequest(traveler);
        if (request is null)
            return TurnOutcome.Cancelled;

        controller.ResolveSkill(traveler, request);
        return TurnOutcome.Completed;
    }
}
