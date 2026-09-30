namespace Octopath_Traveler;

public class HealEffect : ISkillEffect
{
    private readonly double _modifier;

    public HealEffect(double modifier)
        => _modifier = modifier;

    public IReadOnlyList<SkillOutcome> Apply(SkillContext context, IReadOnlyList<Unit> targets)
    {
        int amount = CalculateAmount(context.User);
        List<SkillOutcome> outcomes = new();
        foreach (Unit target in targets)
        {
            if (!target.IsAlive)
                continue;
            target.Heal(amount);
            outcomes.Add(new HealOutcome(target, amount));
        }
        return outcomes;
    }

    private int CalculateAmount(Unit user)
        => (int)Math.Floor(user.Stats.ElemDef * _modifier);
}
