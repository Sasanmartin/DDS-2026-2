namespace Octopath_Traveler;

public abstract class PassiveSkill
{
    protected PassiveSkill(string name)
        => Name = name;

    public string Name { get; }

    public abstract void ApplyTo(Stats stats);

    public virtual void OnRoundEnd(Traveler bearer) { }

    public virtual bool GrantsExtraTurn(Traveler bearer) => false;

    public virtual void OnCombatStart(Traveler bearer) { }
}

public abstract class StatBoostPassive : PassiveSkill
{
    protected StatBoostPassive(string name) : base(name) { }

    protected virtual int Hp => 0;
    protected virtual int Sp => 0;
    protected virtual int PhysAtk => 0;
    protected virtual int ElemAtk => 0;
    protected virtual int Speed => 0;

    public override void ApplyTo(Stats stats)
    {
        stats.HP += Hp;
        stats.SP += Sp;
        stats.PhysAtk += PhysAtk;
        stats.ElemAtk += ElemAtk;
        stats.Speed += Speed;
    }
}

public class NullPassive : PassiveSkill
{
    public NullPassive(string name) : base(name) { }

    public override void ApplyTo(Stats stats) { }
}
