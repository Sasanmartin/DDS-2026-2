namespace Octopath_Traveler;

public class DefendAction : ITravelerAction
{
    public TurnOutcome Execute(CombatController controller, Traveler traveler)
    {
        controller.Defend(traveler);
        return TurnOutcome.Completed;
    }
}
