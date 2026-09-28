namespace Letters.Effects;

/// <summary>Efecto "f": +2 Atk, +2 Res, +2 Spd.</summary>
public class EffectF : StatBoostEffect
{
    protected override int Atk => 2;
    protected override int Res => 2;
    protected override int Spd => 2;
}
