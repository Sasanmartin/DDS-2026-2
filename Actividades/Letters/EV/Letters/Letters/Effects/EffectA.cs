namespace Letters.Effects;

public class EffectA : Effect
{
    public override void Apply(Stats stats) => stats.Atk += 5;
}