namespace Letters.Effects;

public class EffectD : Effect
{
    public override void Apply(Stats stats) => stats.Def += 3;
}