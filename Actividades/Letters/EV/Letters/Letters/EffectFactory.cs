using Letters.Effects;

namespace Letters;

public static class EffectFactory
{
    public static Effect Create(char action)
    {
        return action switch 
            // Creacion de la clase abstracta para que funcione el switch (logica permitido por la herencia)
            // El hecho de que sea abstracta es porque heredan un unico metodo que todos sobreescriben.
        {
            'a' => new EffectA(),
            'b' => new EffectB(),
            'c' => new EffectC(),
            'd' => new EffectD(),
            'e' => new EffectE(),
            'f' => new EffectF(),
            'g' => new EffectG(),
            _ => throw new ArgumentException($"Acción de efecto desconocida: {action}")
        };
    }
}