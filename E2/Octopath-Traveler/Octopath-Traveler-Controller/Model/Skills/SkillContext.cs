namespace Octopath_Traveler;

public class SkillContext
{
    public SkillContext(Unit user, CombatState state, Unit? chosenTarget = null)
    {
        User = user;
        State = state;
        ChosenTarget = chosenTarget;
    }

    public Unit User { get; }
    public CombatState State { get; }
    public Unit? ChosenTarget { get; }
    public AttackType? ChosenWeapon { get; init; }
}
