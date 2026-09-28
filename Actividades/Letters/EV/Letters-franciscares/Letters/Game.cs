using Letters.Effects;

namespace Letters;

/// <summary>
/// Recorre el string de acciones y aplica el efecto de cada letra.
///
/// Game ya no conoce ningun efecto concreto: solo le pide uno a la factory
/// y lo aplica. Por eso agregar efectos nuevos no obliga a tocar esta clase
/// (open-closed principle).
/// </summary>
public class Game
{
    private readonly string _actions;
    private readonly EffectFactory _factory;

    public Game(string actions)
        : this(actions, new EffectFactory())
    {
    }

    /// <summary>
    /// Permite inyectar una factory propia (por ejemplo, una con efectos extra
    /// registrados) sin modificar el codigo del juego.
    /// </summary>
    public Game(string actions, EffectFactory factory)
    {
        _actions = actions;
        _factory = factory;
    }

    public string Play()
    {
        var stats = new Stats();

        foreach (var action in _actions)
        {
            _factory.Create(action).ApplyTo(stats);
        }

        return stats.ToString();
    }
}
