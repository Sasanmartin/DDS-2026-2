namespace Octopath_Traveler;

public class ChosenLivingOpponentSelector : IChoosableTargetSelector
{
    public IReadOnlyList<Unit> CandidateTargets(SkillContext context)
        => context.State.LivingOpponentsOf(context.User);

    public IReadOnlyList<Unit> SelectTargets(SkillContext context)
        => IsValid(context) ? new[] { context.ChosenTarget! } : Array.Empty<Unit>();

    private static bool IsValid(SkillContext context)
    {
        Unit? target = context.ChosenTarget;
        return target is not null && target.IsAlive
            && context.State.LivingOpponentsOf(context.User).Contains(target);
    }
}

public class ChosenLivingAllySelector : IChoosableTargetSelector
{
    public IReadOnlyList<Unit> CandidateTargets(SkillContext context)
        => context.State.LivingAlliesOf(context.User);

    public IReadOnlyList<Unit> SelectTargets(SkillContext context)
        => IsValid(context) ? new[] { context.ChosenTarget! } : Array.Empty<Unit>();

    private static bool IsValid(SkillContext context)
    {
        Unit? target = context.ChosenTarget;
        return target is not null && target.IsAlive
            && context.State.LivingAlliesOf(context.User).Contains(target);
    }
}

public class ChosenFallenAllySelector : IChoosableTargetSelector
{
    public IReadOnlyList<Unit> CandidateTargets(SkillContext context)
        => context.State.FallenAlliesOf(context.User);

    public IReadOnlyList<Unit> SelectTargets(SkillContext context)
        => IsValid(context) ? new[] { context.ChosenTarget! } : Array.Empty<Unit>();

    private static bool IsValid(SkillContext context)
    {
        Unit? target = context.ChosenTarget;
        return target is not null && !target.IsAlive
            && context.State.FallenAlliesOf(context.User).Contains(target);
    }
}
