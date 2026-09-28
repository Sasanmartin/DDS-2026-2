namespace Letters.Effects;

public class EffectG : Effect
{
    public override void Apply(Stats stats)
    {
        stats.Atk += 1;
        stats.Def += 1;
        stats.Res += 1;
        stats.Spd += 1;
    }
}