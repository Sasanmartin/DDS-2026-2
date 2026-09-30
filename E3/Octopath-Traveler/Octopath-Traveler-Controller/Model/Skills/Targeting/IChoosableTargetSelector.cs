namespace Octopath_Traveler;

public interface IChoosableTargetSelector : ITargetSelector
{
    IReadOnlyList<Unit> CandidateTargets(SkillContext context);
}
