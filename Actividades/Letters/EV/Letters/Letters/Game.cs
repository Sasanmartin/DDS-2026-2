using Letters.Effects;

namespace Letters;

public class Game
{
    private readonly Stats _stats = new();
    private string _actions;

    public Game(string actions)
        => _actions = actions;

    public string Play()
    {
        foreach (var action in _actions)
        {
            Effect effect = EffectFactory.Create(action);
            effect.Apply(_stats);
        }

        return _stats.GetStats();
    }
}