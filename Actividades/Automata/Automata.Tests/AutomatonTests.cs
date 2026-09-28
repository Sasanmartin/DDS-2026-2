using System.IO;
using Xunit;

namespace Automata.Tests;

public class AutomatonTests
{
    [Theory]
    [InlineData("01.txt", "abc")]
    [InlineData("01.txt", "sdwaagfbfscds")]
    [InlineData("02.txt", "")]
    [InlineData("02.txt", "aaa")]
    public void IsWordValid_TheseWordsShouldBeValid(string fileDFA, string word)
    {
        fileDFA = GetPathToDFA(fileDFA);
        Automaton dfa = new AutomatonFactory().CreateFromFile(fileDFA);

        bool isWordValid = dfa.Accepts(word);

        Assert.True(isWordValid);
    }

    [Theory]
    [InlineData("01.txt", "cab")]
    [InlineData("01.txt", "sdwaagfbfsds")]
    [InlineData("02.txt", "ab")]
    [InlineData("02.txt", "cradsbdcdsb")]
    public void IsWordValid_TheseWordsShouldBeInvalid(string fileDFA, string word)
    {
        fileDFA = GetPathToDFA(fileDFA);
        Automaton dfa = new AutomatonFactory().CreateFromFile(fileDFA);

        bool isWordValid = dfa.Accepts(word);

        Assert.False(isWordValid);
    }

    private string GetPathToDFA(string dfa)
        => Path.Combine("DFAs", dfa);
}
