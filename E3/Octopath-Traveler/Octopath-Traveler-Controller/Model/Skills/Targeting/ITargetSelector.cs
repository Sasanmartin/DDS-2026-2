namespace Octopath_Traveler;

public interface ITargetSelector
{
    IReadOnlyList<Unit> SelectTargets(SkillContext context);
}
