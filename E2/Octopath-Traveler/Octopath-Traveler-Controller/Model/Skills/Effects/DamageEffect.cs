namespace Octopath_Traveler;

public class DamageEffect : ISkillEffect
{
    private readonly DamageProfile _profile;
    private readonly DamageCalculator _calculator;

    public DamageEffect(DamageProfile profile, DamageCalculator calculator)
    {
        _profile = profile;
        _calculator = calculator;
    }

    public IReadOnlyList<SkillOutcome> Apply(SkillContext context, IReadOnlyList<Unit> targets)
    {
        List<SkillOutcome> outcomes = new();
        foreach (Unit target in targets)
            ApplyToTarget(context, target, outcomes);
        return outcomes;
    }

    private void ApplyToTarget(SkillContext context, Unit target, List<SkillOutcome> outcomes)
    {
        foreach (AttackType type in _profile.Hits)
            outcomes.Add(DealDamage(context, target, type));
    }

    private DamageOutcome DealDamage(SkillContext context, Unit target, AttackType type)
    {
        int damage = CalculateDamage(context, target, type);
        bool weakness = TargetsWeakness(target, type);
        bool enteredBreakingPoint = weakness && damage > 0 && TryBreakShield(target, context.State.Round);
        target.TakeDamage(damage);
        return new DamageOutcome(context.User, target, damage, type, weakness, enteredBreakingPoint);
    }

    private int CalculateDamage(SkillContext context, Unit target, AttackType type)
        => _calculator.CalculateDamage(
            new DamageRequest(context.User, target, type, _profile));

    private static bool TargetsWeakness(Unit target, AttackType type)
        => target is Beast beast && beast.IsWeakTo(type);

    private static bool TryBreakShield(Unit target, int round)
    {
        if (target is not Beast beast || beast.IsInBreakingPoint)
            return false;
        beast.LoseShield();
        if (beast.Shields > 0)
            return false;

        beast.EnterBreakingPoint(round);
        return true;
    }
}
