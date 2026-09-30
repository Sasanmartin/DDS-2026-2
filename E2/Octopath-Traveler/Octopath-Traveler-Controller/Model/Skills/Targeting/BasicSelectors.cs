namespace Octopath_Traveler;

public class UserTargetSelector : ITargetSelector
{
    public IReadOnlyList<Unit> SelectTargets(SkillContext context)
        => new[] { context.User };
}

public class AllLivingOpponentsSelector : ITargetSelector
{
    public IReadOnlyList<Unit> SelectTargets(SkillContext context)
        => context.State.LivingOpponentsOf(context.User);
}

public class AllLivingAlliesSelector : ITargetSelector
{
    public IReadOnlyList<Unit> SelectTargets(SkillContext context)
    {
        List<Unit> allies = context.State.LivingAlliesOf(context.User)
            .Where(ally => ally != context.User)
            .ToList();
        allies.Add(context.User);
        return allies;
    }
}

public class AllFallenAlliesSelector : ITargetSelector
{
    public IReadOnlyList<Unit> SelectTargets(SkillContext context)
        => context.State.FallenAlliesOf(context.User);
}
