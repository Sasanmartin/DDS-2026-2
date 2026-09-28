namespace PrimeNumbers;

public class GeneradorDePrimos
{
    private bool[] _tachados = Array.Empty<bool>();
    private int[] _primos = Array.Empty<int>();

    public int[] GenerarPrimos(int valorMaximo)
    {
        if (valorMaximo < 2)
            return Array.Empty<int>();

        InicializarCandidatosHasta(valorMaximo);
        TacharMultiplos();
        PonerPrimosEnResultado();
        return _primos;
    }

    private void InicializarCandidatosHasta(int valorMaximo)
        => _tachados = new bool[valorMaximo + 1];

    private void TacharMultiplos()
    {
        int mayorPrimoATachar = ObtenerMayorPrimoATachar();
        for (int candidato = 2; candidato <= mayorPrimoATachar; candidato++)
            if (EstaSinTachar(candidato))
                TacharMultiplosDe(candidato);
    }

    private int ObtenerMayorPrimoATachar()
    {
        double raizCuadrada = Math.Sqrt(_tachados.Length);
        return (int)raizCuadrada + 1;
    }

    private bool EstaSinTachar(int candidato)
        => !_tachados[candidato];

    private void TacharMultiplosDe(int primo)
    {
        for (int multiplo = 2 * primo; multiplo < _tachados.Length; multiplo += primo)
            _tachados[multiplo] = true;
    }

    private void PonerPrimosEnResultado()
    {
        _primos = new int[ContarPrimos()];
        int indice = 0;
        for (int candidato = 2; candidato < _tachados.Length; candidato++)
            if (EstaSinTachar(candidato))
                _primos[indice++] = candidato;
    }

    private int ContarPrimos()
    {
        int conteo = 0;
        for (int candidato = 2; candidato < _tachados.Length; candidato++)
            if (EstaSinTachar(candidato))
                conteo++;
        return conteo;
    }
}
