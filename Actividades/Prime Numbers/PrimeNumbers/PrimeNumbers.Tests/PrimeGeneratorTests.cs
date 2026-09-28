using Xunit;

namespace PrimeNumbers.Tests;

public class PrimeGeneratorTests
{

    [Fact]
    public void Generate_ShouldReturnAnEmptyArrayWhenMaxValueIsLowerThanTwo()
    {
        GeneradorDePrimos generador = new();
        int expected = 0;

        int[] actual = generador.GenerarPrimos(1);
        
        Assert.Equal(expected, actual.Length);
    }

    [Fact]
    public void Generate_ShouldFindTheRightNumberOfPrimeNumbersLowerThan100()
    {
        GeneradorDePrimos generador = new();
        int[] expected = GetPrimesLowerThan100();
        int maxValue = 100;

        int[] actual = generador.GenerarPrimos(maxValue);

        Assert.Equal(expected.Length, actual.Length);
    }

    [Fact]
    public void Generate_ShouldFindAllThePrimeNumbersLowerThan100()
    {
        // Arrange
        GeneradorDePrimos generador = new();
        int[] expected = GetPrimesLowerThan100();
        int maxValue = 100;

        // Act 
        int[] actual = generador.GenerarPrimos(maxValue);
        
        // Assert
        for(int i = 0; i < actual.Length; i++)
            Assert.Equal(expected[i], actual[i]);
    }
    
    private int[] GetPrimesLowerThan100()
        => new int[] {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97};
}