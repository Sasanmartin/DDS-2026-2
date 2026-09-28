namespace Letters.Effects;

/// <summary>
/// Unico punto del sistema que conoce la relacion letra -> efecto.
///
/// Para agregar un efecto nuevo basta con:
///   1. crear una clase que herede de Effect (o de StatBoostEffect), y
///   2. agregar una linea al diccionario de abajo.
/// Ninguna otra clase (Game, Stats, ni los efectos existentes) se modifica.
///
/// Tambien es posible registrar efectos desde afuera con <see cref="Register"/>,
/// sin tocar este archivo.
/// </summary>
public class EffectFactory
{
    private readonly Dictionary<char, Func<Effect>> _effects = new()
    {
        ['a'] = () => new EffectA(),
        ['b'] = () => new EffectB(),
        ['c'] = () => new EffectC(),
        ['d'] = () => new EffectD(),
        ['e'] = () => new EffectE(),
        ['f'] = () => new EffectF(),
        ['g'] = () => new EffectG(),
    };

    /// <summary>
    /// Asocia una letra a un efecto. Si la letra ya existia, la reemplaza.
    /// </summary>
    public EffectFactory Register(char action, Func<Effect> effect)
    {
        _effects[action] = effect;
        return this;
    }

    /// <summary>
    /// Devuelve el efecto de la letra pedida, o un NoEffect si la letra no existe.
    /// </summary>
    public Effect Create(char action)
        => _effects.TryGetValue(action, out var effect) ? effect() : new NoEffect();
}
