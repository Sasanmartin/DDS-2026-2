namespace Octopath_Traveler;

public class DamageCalculator
{
    public int CalculateDamage(Unit attacker, Unit target, double modifier)
    {
        double rawDamage = attacker.Stats.PhysAtk * modifier - target.Stats.PhysDef;
        return Math.Max(0, (int)Math.Floor(rawDamage));
    }
}
