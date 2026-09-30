namespace Octopath_Traveler;

public class DamageCalculator
{
    private const double WeaknessBonus = 0.5;
    private const double BreakingPointBonus = 0.5;
    private const double DefendingReduction = 0.5;
    private const double NoBonus = 1;

    public int CalculateDamage(DamageRequest request)
    {
        double modifier = request.Profile.Modifier;
        double offense = OffensiveStatOf(request.Attacker, request.Type);
        double defense = DefensiveStatOf(request.Target, request.Type);
        double baseDamage = request.Profile.ApplyBonus(offense * modifier - defense, request.Attacker);
        double multiplier = DamageMultiplier(request.Target, request.Type);
        double damage = Math.Max(0, baseDamage) * multiplier;
        return request.Profile.ApplyCap(Truncate(damage), request.Target);
    }

    private static int Truncate(double value)
        => (int)Math.Floor(value);

    private static double OffensiveStatOf(Unit unit, AttackType type)
        => type.IsPhysical ? unit.Stats.PhysAtk : unit.Stats.ElemAtk;

    private static double DefensiveStatOf(Unit unit, AttackType type)
        => type.IsPhysical ? unit.Stats.PhysDef : unit.Stats.ElemDef;

    private static double DamageMultiplier(Unit target, AttackType type)
    {
        // Multiplicador final: 1 + 0.5 por debilidad + 0.5 por Breaking Point; Defender lo reduce a la mitad.
        double multiplier = NoBonus;
        if (target is Beast beast)
            multiplier += BeastMultiplierBonus(beast, type);
        if (target.IsDefending)
            multiplier *= DefendingReduction;
        return multiplier;
    }

    private static double BeastMultiplierBonus(Beast beast, AttackType type)
    {
        double bonus = 0;
        if (beast.IsWeakTo(type))
            bonus += WeaknessBonus;
        if (beast.IsInBreakingPoint)
            bonus += BreakingPointBonus;
        return bonus;
    }
}
