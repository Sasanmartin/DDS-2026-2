namespace Letters.Effects;

/// <summary>
/// Contrato de todo efecto del juego. Un efecto sabe modificar los Stats,
/// y nadie mas necesita saber que hace por dentro.
///
/// Un efecto complejo (multiplicar, condicionar, combinar otros efectos)
/// hereda directamente de esta clase y sobreescribe ApplyTo.
/// Un efecto que solo suma valores fijos conviene que herede de
/// <see cref="StatBoostEffect"/>.
/// </summary>
public abstract class Effect
{
    public abstract void ApplyTo(Stats stats);
}
