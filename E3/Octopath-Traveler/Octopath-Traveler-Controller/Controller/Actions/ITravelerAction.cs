namespace Octopath_Traveler;

public interface ITravelerAction
{
    TurnOutcome Execute(ActionSelector selector, CombatController controller, Traveler traveler);
}
