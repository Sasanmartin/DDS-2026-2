namespace Octopath_Traveler;

public class ActiveSkillFactory
{
    private const string HealKeyword = "Restaura HP";
    private const string TargetSingle = "Single";
    private const string TargetEnemies = "Enemies";
    private const string TargetUser = "User";
    private const string TargetAlly = "Ally";
    private const string TargetParty = "Party";
    private const string ReviveSkill = "Revive";
    private const string VivifySkill = "Vivify";
    private const string LegholdTrapSkill = "Leghold Trap";
    private const string SpearheadSkill = "Spearhead";
    private const string LastStandSkill = "Last Stand";
    private const string MercyStrikeSkill = "Mercy Strike";
    private const string ShootingStarsSkill = "Shooting Stars";
    private const string NightmareChimeraSkill = "Nightmare Chimera";
    private const string HpThiefSkill = "HP Thief";
    private const string WindType = "Wind";
    private const string LightType = "Light";
    private const string DarkType = "Dark";
    private const int LegholdRounds = 2;
    private const int SpearheadRounds = 1;
    private const int HpThiefHits = 2;

    private static readonly AttackType[] ShootingStarsHits =
    {
        AttackType.FromName(WindType),
        AttackType.FromName(LightType),
        AttackType.FromName(DarkType)
    };

    private readonly Dictionary<string, ActiveSkillData> _data;
    private readonly DamageCalculator _calculator;
    private readonly Dictionary<string, Func<ActiveSkillData, ActiveSkill>> _specialCreators;

    public ActiveSkillFactory(Dictionary<string, ActiveSkillData> data, DamageCalculator calculator)
    {
        _data = data;
        _calculator = calculator;
        _specialCreators = new()
        {
            [ReviveSkill] = BuildRevive,
            [VivifySkill] = BuildVivify,
            [LegholdTrapSkill] = BuildLegholdTrap,
            [SpearheadSkill] = BuildSpearhead,
            [LastStandSkill] = BuildLastStand,
            [MercyStrikeSkill] = BuildMercyStrike,
            [ShootingStarsSkill] = BuildShootingStars,
            [NightmareChimeraSkill] = BuildNightmareChimera,
            [HpThiefSkill] = BuildHpThief
        };
    }

    public ActiveSkill Create(string name)
    {
        if (!_data.TryGetValue(name, out ActiveSkillData? data))
            return BuildWithoutEffects(name, spCost: 0);

        return _specialCreators.TryGetValue(name, out Func<ActiveSkillData, ActiveSkill>? creator)
            ? creator(data)
            : BuildFromData(data);
    }

    private ActiveSkill BuildFromData(ActiveSkillData data)
    {
        if (HasDamageType(data))
            return BuildDamage(data);
        if (IsHealing(data))
            return BuildHeal(data);
        return BuildWithoutEffects(data.Name, data.SP);
    }

    private ActiveSkill BuildDamage(ActiveSkillData data)
        => BuildSingleHit(data, SingleHitProfile(AttackType.FromName(data.Type), data.Modifier));

    private static ActiveSkill BuildHeal(ActiveSkillData data)
        => new(data.Name, data.SP, SelectorFor(data.Target),
            new ISkillEffect[] { new HealEffect(data.Modifier) });

    private static ActiveSkill BuildRevive(ActiveSkillData data)
        => new(data.Name, data.SP, new AllFallenAlliesSelector(),
            new ISkillEffect[] { new ReviveEffect() });

    private static ActiveSkill BuildVivify(ActiveSkillData data)
        => new(data.Name, data.SP, new ChosenFallenAllySelector(),
            new ISkillEffect[] { new ReviveEffect(), new HealEffect(data.Modifier) });

    private static ActiveSkill BuildLegholdTrap(ActiveSkillData data)
        => new(data.Name, data.SP, new ChosenLivingOpponentSelector(),
            new ISkillEffect[] { new DecreasePriorityEffect(LegholdRounds) });

    private ActiveSkill BuildSpearhead(ActiveSkillData data)
    {
        DamageProfile profile = SingleHitProfile(AttackType.FromName(data.Type), data.Modifier);
        return new ActiveSkill(data.Name, data.SP, new ChosenLivingOpponentSelector(),
            new ISkillEffect[]
            {
                new DamageEffect(profile, _calculator),
                new UserPriorityEffect(SpearheadRounds)
            });
    }

    private ActiveSkill BuildLastStand(ActiveSkillData data)
        => BuildSingleHit(data, SingleHitProfile(AttackType.FromName(data.Type), data.Modifier,
            bonus: new MissingHpDamageBonus()));

    private ActiveSkill BuildMercyStrike(ActiveSkillData data)
        => BuildSingleHit(data, SingleHitProfile(AttackType.FromName(data.Type), data.Modifier,
            cap: new LeaveAtLeastOneHpCap()));

    private ActiveSkill BuildShootingStars(ActiveSkillData data)
        => BuildSingleHit(data, new DamageProfile
        {
            Hits = ShootingStarsHits,
            Modifier = data.Modifier
        });

    private ActiveSkill BuildNightmareChimera(ActiveSkillData data)
        => new(data.Name, data.SP, SelectorFor(data.Target),
            new ISkillEffect[] { new ChosenWeaponDamageEffect(data.Modifier, _calculator) });

    private ActiveSkill BuildHpThief(ActiveSkillData data)
        => BuildSingleHit(data, new DamageProfile
        {
            Hits = AttackType.FromName(data.Type).Repeat(HpThiefHits),
            Modifier = data.Modifier
        });

    private ActiveSkill BuildSingleHit(ActiveSkillData data, DamageProfile profile)
        => new(data.Name, data.SP, SelectorFor(data.Target),
            new ISkillEffect[] { new DamageEffect(profile, _calculator) });

    private static DamageProfile SingleHitProfile(AttackType type, double modifier,
        IDamageBonus? bonus = null, IDamageCap? cap = null)
        => new()
        {
            Hits = new[] { type },
            Modifier = modifier,
            DamageBonus = bonus ?? new NoDamageBonus(),
            DamageCap = cap ?? new NoDamageCap()
        };

    private static ActiveSkill BuildWithoutEffects(string name, int spCost)
        => new(name, spCost, new UserTargetSelector(), Array.Empty<ISkillEffect>());

    private static bool HasDamageType(ActiveSkillData data)
        => data.Type.Length > 0;

    private static bool IsHealing(ActiveSkillData data)
        => data.Description.Contains(HealKeyword, StringComparison.OrdinalIgnoreCase);

    private static ITargetSelector SelectorFor(string target)
        => target switch
        {
            TargetSingle => new ChosenLivingOpponentSelector(),
            TargetEnemies => new AllLivingOpponentsSelector(),
            TargetUser => new UserTargetSelector(),
            TargetAlly => new ChosenLivingAllySelector(),
            TargetParty => new AllLivingAlliesSelector(),
            _ => new UserTargetSelector()
        };
}
