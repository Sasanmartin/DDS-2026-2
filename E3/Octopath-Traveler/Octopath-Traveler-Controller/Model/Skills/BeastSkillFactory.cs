namespace Octopath_Traveler;

public class BeastSkillFactory
{
    private const string HalveHpSkillName = "Vortal Claw";
    private const string EnemiesTarget = "Enemies";
    private const string HighestElemAtkKeyword = "mayor Elem Atk";
    private const string HighestPhysAtkKeyword = "mayor Phys Atk";
    private const string LowestPhysDefKeyword = "menor Phys Def";
    private const string LowestElemDefKeyword = "menor Elem Def";
    private const string HighestSpeedKeyword = "mayor Speed";
    private const string LowestSpeedKeyword = "menor Speed";

    private static readonly Dictionary<string, Func<ITargetSelector>> SelectorByKeyword = new()
    {
        [HighestElemAtkKeyword] = () => new HighestElemAtkOpponentSelector(),
        [HighestPhysAtkKeyword] = () => new HighestPhysAtkOpponentSelector(),
        [LowestPhysDefKeyword] = () => new LowestPhysDefOpponentSelector(),
        [LowestElemDefKeyword] = () => new LowestElemDefOpponentSelector(),
        [HighestSpeedKeyword] = () => new HighestSpeedOpponentSelector(),
        [LowestSpeedKeyword] = () => new LowestSpeedOpponentSelector()
    };

    private readonly DamageCalculator _calculator;

    public BeastSkillFactory(DamageCalculator calculator)
    {
        _calculator = calculator;
    }

    public ActiveSkill Create(BeastSkill skill)
    {
        if (IsHalveHpSkill(skill))
            return BuildHalveHp(skill);

        DamageProfile profile = new()
        {
            Hits = AttackTypeFor(skill).Repeat(Math.Max(1, skill.Hits)),
            Modifier = skill.Modifier
        };
        return new ActiveSkill(skill.Name, spCost: 0, SelectorFor(skill),
            new ISkillEffect[] { new DamageEffect(profile, _calculator) });
    }

    private static bool IsHalveHpSkill(BeastSkill skill)
        => skill.Name == HalveHpSkillName;

    private static ActiveSkill BuildHalveHp(BeastSkill skill)
        => new(skill.Name, spCost: 0, new AllLivingOpponentsSelector(),
            new ISkillEffect[] { new HalveHpEffect() });

    private static AttackType AttackTypeFor(BeastSkill skill)
        => skill.IsElementalAttack ? AttackType.ElementalAttack : AttackType.PhysicalAttack;

    private static ITargetSelector SelectorFor(BeastSkill skill)
        => skill.Target == EnemiesTarget
            ? new AllLivingOpponentsSelector()
            : SingleTargetSelectorFor(skill.Description);

    private static ITargetSelector SingleTargetSelectorFor(string description)
    {
        foreach ((string keyword, Func<ITargetSelector> create) in SelectorByKeyword)
            if (description.Contains(keyword))
                return create();
        return new HighestHpOpponentSelector();
    }
}
