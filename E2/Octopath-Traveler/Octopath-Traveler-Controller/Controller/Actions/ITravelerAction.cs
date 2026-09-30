namespace Octopath_Traveler;

public interface ITravelerAction
{
    TurnOutcome Execute(CombatController controller, Traveler traveler);
}
