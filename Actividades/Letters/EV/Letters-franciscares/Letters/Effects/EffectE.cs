namespace Letters.Effects;

/// <summary>Efecto "e": +2 Atk, +2 Def, +6 Res.</summary>
public class EffectE : StatBoostEffect
{
    protected override int Atk => 2;
    protected override int Def => 2;
    protected override int Res => 6;
}
