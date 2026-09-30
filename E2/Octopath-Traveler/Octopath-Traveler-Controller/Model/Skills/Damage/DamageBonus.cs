namespace Octopath_Traveler;

public interface IDamageBonus
{
    double Apply(double baseDamage, Unit user);
}

public class NoDamageBonus : IDamageBonus
{
    public double Apply(double baseDamage, Unit user) => baseDamage;
}

public class MissingHpDamageBonus : IDamageBonus
{
    private const int PercentTotal = 100;
    private const double BonusPerMissingPercent = 0.03;

    public double Apply(double baseDamage, Unit user)
        => baseDamage * (1 + BonusPerMissingPercent * MissingPercentOf(user));

    // Last Stand: +3% de daño por cada 1% de HP faltante, con el porcentaje entero truncado.
    private static int MissingPercentOf(Unit user)
        => (user.MaxHp - user.CurrentHp) * PercentTotal / user.MaxHp;
}
