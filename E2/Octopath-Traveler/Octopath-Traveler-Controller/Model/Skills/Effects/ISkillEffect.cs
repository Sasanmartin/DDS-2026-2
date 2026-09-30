namespace Octopath_Traveler;

public interface ISkillEffect
{
    bool RequiresWeaponChoice => false;

    IReadOnlyList<SkillOutcome> Apply(SkillContext context, IReadOnlyList<Unit> targets);
}
