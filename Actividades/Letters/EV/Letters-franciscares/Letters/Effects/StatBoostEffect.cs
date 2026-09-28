namespace Letters.Effects;

/// <summary>
/// Base para los efectos que simplemente suman una cantidad fija a los atributos.
/// La subclase solo declara los atributos que le interesan; el resto queda en 0.
/// </summary>
public abstract class StatBoostEffect : Effect
{
    protected virtual int Atk => 0;
    protected virtual int Def => 0;
    protected virtual int Res => 0;
    protected virtual int Spd => 0;

    public override void ApplyTo(Stats stats)
        => stats.Add(atk: Atk, def: Def, res: Res, spd: Spd);
}
