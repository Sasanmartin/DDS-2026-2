namespace Letters.Effects;

public class EffectE : Effect
{
    public override void Apply(Stats stats)
    {
        stats.Atk += 2;
        stats.Def += 2;
        stats.Res += 6;
    }
}