namespace Letters;

/// <summary>
/// Guarda los cuatro atributos del juego. Todos parten en cero.
/// Los efectos son los unicos que modifican este estado.
/// </summary>
public class Stats
{
    public int Atk { get; set; }
    public int Def { get; set; }
    public int Res { get; set; }
    public int Spd { get; set; }

    /// <summary>
    /// Suma (o resta, si el valor es negativo) a cada atributo.
    /// Los parametros con nombre permiten escribir solo los atributos afectados.
    /// </summary>
    public void Add(int atk = 0, int def = 0, int res = 0, int spd = 0)
    {
        Atk += atk;
        Def += def;
        Res += res;
        Spd += spd;
    }

    /// <summary>
    /// Formato de salida del juego: Atk-Spd-Res-Def.
    /// </summary>
    public override string ToString()
        => $"{Atk}-{Spd}-{Res}-{Def}";
}
