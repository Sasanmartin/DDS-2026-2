namespace Letters.Effects;

public class EffectF : Effect
{
    public override void Apply(Stats stats)
    {
        stats.Atk += 2;
        stats.Res += 2;
        stats.Spd += 2;
    }
}