namespace Letters.Effects;

/// <summary>Efecto "g": +1 a todos los atributos.</summary>
public class EffectG : StatBoostEffect
{
    protected override int Atk => 1;
    protected override int Def => 1;
    protected override int Res => 1;
    protected override int Spd => 1;
}
