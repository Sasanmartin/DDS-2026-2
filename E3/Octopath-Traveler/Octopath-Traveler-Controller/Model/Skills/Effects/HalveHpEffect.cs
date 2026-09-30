namespace Octopath_Traveler;

public class HalveHpEffect : ISkillEffect
{
    public IReadOnlyList<SkillOutcome> Apply(SkillContext context, IReadOnlyList<Unit> targets)
    {
        List<SkillOutcome> outcomes = new();
        foreach (Unit target in targets)
        {
            if (!target.IsAlive)
                continue;
            int damage = target.CurrentHp - target.CurrentHp / 2;
            target.TakeDamage(damage);
            outcomes.Add(new DamageOutcome(context.User, target, damage, AttackType.None, false, false));
        }
        return outcomes;
    }
}
