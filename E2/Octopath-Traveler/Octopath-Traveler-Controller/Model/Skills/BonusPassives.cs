namespace Octopath_Traveler;

public class VimAndVigorPassive : PassiveSkill
{
    private const double HealPercent = 0.10;

    public VimAndVigorPassive() : base("Vim and Vigor") { }

    public override void ApplyTo(Stats stats) { }

    public override void OnRoundEnd(Traveler bearer)
        => bearer.Heal((int)Math.Floor(bearer.MaxHp * HealPercent));
}

public class SecondWindPassive : PassiveSkill
{
    private const double RecoveryPercent = 0.05;

    public SecondWindPassive() : base("Second Wind") { }

    public override void ApplyTo(Stats stats) { }

    public override void OnRoundEnd(Traveler bearer)
        => bearer.RecoverSp((int)Math.Floor(bearer.MaxSp * RecoveryPercent));
}

public class PatiencePassive : PassiveSkill
{
    public PatiencePassive() : base("Patience") { }

    public override void ApplyTo(Stats stats) { }

    public override bool GrantsExtraTurn(Traveler bearer)
        => bearer.CurrentHp % 2 == 0 && bearer.CurrentSp % 2 == 0;
}

public class BoostStartPassive : PassiveSkill
{
    public BoostStartPassive() : base("Boost Start") { }

    public override void ApplyTo(Stats stats) { }

    public override void OnCombatStart(Traveler bearer)
        => bearer.GrantExtraBoostPoint();
}

public class StatSwapPassive : PassiveSkill
{
    public StatSwapPassive() : base("Stat Swap") { }

    public override void ApplyTo(Stats stats)
        => (stats.PhysAtk, stats.ElemAtk) = (stats.ElemAtk, stats.PhysAtk);
}
