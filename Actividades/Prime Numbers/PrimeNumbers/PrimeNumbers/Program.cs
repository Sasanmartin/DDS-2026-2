using PrimeNumbers;

GeneradorDePrimos generador = new();
int[] primos = generador.GenerarPrimos(20);
foreach (int primo in primos)
    Console.WriteLine(primo);
