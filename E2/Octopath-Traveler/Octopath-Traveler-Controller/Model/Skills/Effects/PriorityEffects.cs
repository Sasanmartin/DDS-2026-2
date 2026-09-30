namespace Octopath_Traveler;

public class DecreasePriorityEffect : ISkillEffect
{
    private readonly int _rounds;

    public DecreasePriorityEffect(int rounds)
        => _rounds = rounds;

    public IReadOnlyList<SkillOutcome> Apply(SkillContext context, IReadOnlyList<Unit> targets)
    {
        foreach (Unit target in targets)
            target.DecreasePriorityFrom(context.State.Round);

        return targets
            .Select(target => (SkillOutcome)new PriorityOutcome(target, _rounds, false))
            .ToList();
    }
}

public class UserPriorityEffect : ISkillEffect
{
    private readonly int _rounds;

    public UserPriorityEffect(int rounds)
        => _rounds = rounds;

    public IReadOnlyList<SkillOutcome> Apply(SkillContext context, IReadOnlyList<Unit> targets)
    {
        context.User.IncreasePriorityForRound(context.State.Round + 1);
        return new SkillOutcome[] { new PriorityOutcome(context.User, _rounds, true) };
    }
}
