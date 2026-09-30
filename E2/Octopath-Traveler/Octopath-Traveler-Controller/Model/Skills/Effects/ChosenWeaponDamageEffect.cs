namespace Octopath_Traveler;

public class ChosenWeaponDamageEffect : ISkillEffect
{
    private readonly double _modifier;
    private readonly DamageCalculator _calculator;

    public ChosenWeaponDamageEffect(double modifier, DamageCalculator calculator)
    {
        _modifier = modifier;
        _calculator = calculator;
    }

    public bool RequiresWeaponChoice => true;

    public IReadOnlyList<SkillOutcome> Apply(SkillContext context, IReadOnlyList<Unit> targets)
    {
        AttackType type = context.ChosenWeapon ?? AttackType.PhysicalAttack;
        DamageProfile profile = new()
        {
            Hits = new[] { type },
            Modifier = _modifier
        };
        return new DamageEffect(profile, _calculator).Apply(context, targets);
    }
}
