namespace Letters.Effects;

/// <summary>
/// Null Object: se usa para las letras que no tienen efecto asociado.
/// Mantiene el comportamiento del codigo original (ignorar caracteres desconocidos)
/// sin obligar al Game a preguntar si el efecto existe.
/// </summary>
public class NoEffect : Effect
{
    public override void ApplyTo(Stats stats)
    {
        // No hace nada a proposito.
    }
}
