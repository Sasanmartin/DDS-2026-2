namespace Octopath_Traveler;

public class SummonStrengthPassive : StatBoostPassive
{
    private const int Bonus = 50;

    public SummonStrengthPassive() : base("Summon Strength") { }

    protected override int PhysAtk => Bonus;
}

public class ElementalAugmentationPassive : StatBoostPassive
{
    private const int Bonus = 50;

    public ElementalAugmentationPassive() : base("Elemental Augmentation") { }

    protected override int ElemAtk => Bonus;
}

public class HaleAndHeartyPassive : StatBoostPassive
{
    private const int Bonus = 500;

    public HaleAndHeartyPassive() : base("Hale and Hearty") { }

    protected override int Hp => Bonus;
}

public class FleefootPassive : StatBoostPassive
{
    private const int Bonus = 50;

    public FleefootPassive() : base("Fleefoot") { }

    protected override int Speed => Bonus;
}

public class InnerStrengthPassive : StatBoostPassive
{
    private const int Bonus = 50;

    public InnerStrengthPassive() : base("Inner Strength") { }

    protected override int Sp => Bonus;
}
