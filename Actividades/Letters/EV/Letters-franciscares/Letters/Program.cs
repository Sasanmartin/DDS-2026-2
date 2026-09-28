using Letters;
using Letters.Effects;

// Uso normal del juego.
Console.WriteLine(new Game("abcdefg").Play()); // 10-5-16-6

// Demostracion del open-closed principle:
// EffectH (definido mas abajo) es un efecto nuevo. Para usarlo no fue
// necesario modificar Game, Stats, EffectFactory ni ningun efecto existente.
var factory = new EffectFactory()
    .Register('h', () => new EffectH());

Console.WriteLine(new Game("hh", factory).Play()); // 20-0-0-0

/// <summary>Efecto nuevo "h": +10 Atk. Agregado sin tocar el resto del codigo.</summary>
public class EffectH : StatBoostEffect
{
    protected override int Atk => 10;
}
