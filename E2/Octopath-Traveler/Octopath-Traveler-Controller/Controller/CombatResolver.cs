namespace Octopath_Traveler;

public class CombatResolver
{
    private const double BasicAttackModifier = 1.3;

    private readonly CombatState _state;
    private readonly DamageCalculator _calculator = new();
    private readonly BeastSkillFactory _beastSkills;

    public CombatResolver(CombatState state)
    {
        _state = state;
        _beastSkills = new BeastSkillFactory(_calculator);
    }

    public IReadOnlyList<SkillOutcome> ResolveBasicAttack(Traveler traveler, BasicAttackRequest request)
    {
        traveler.ConsumeBoostPoints(request.BoostPoints);

        DamageProfile profile = new()
        {
            Hits = Enumerable.Repeat(AttackType.FromName(request.Weapon), 1 + request.BoostPoints).ToList(),
            Modifier = BasicAttackModifier
        };
        return ApplyDamage(traveler, request.Target, profile);
    }

    public IReadOnlyList<SkillOutcome> ResolveSkill(Traveler traveler, SkillRequest request)
    {
        traveler.SpendSp(request.Skill.SpCost);
        SkillContext context = new(traveler, _state, request.Target) { ChosenWeapon = request.Weapon };
        return request.Skill.Execute(context);
    }

    public IReadOnlyList<SkillOutcome> ResolveBeastTurn(Beast beast)
    {
        ActiveSkill action = _beastSkills.Create(beast.Skill);
        return action.Execute(new SkillContext(beast, _state));
    }

    private IReadOnlyList<SkillOutcome> ApplyDamage(Unit attacker, Unit target, DamageProfile profile)
        => new DamageEffect(profile, _calculator)
            .Apply(new SkillContext(attacker, _state, target), new[] { target });
}
