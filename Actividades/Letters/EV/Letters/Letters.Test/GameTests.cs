namespace Letters.Test;

public class GameTests
{
    [Theory]
    [InlineData("a", "5-0-0-0")]
    [InlineData("b", "0-2-0-0")]
    [InlineData("c", "0-0-7-0")]
    [InlineData("d", "0-0-0-3")]
    [InlineData("e", "2-0-6-2")]
    [InlineData("f", "2-2-2-0")]
    [InlineData("g", "1-1-1-1")]
    [InlineData("abcdefg", "10-5-16-6")]
    [InlineData("aabbcc", "10-4-14-0")]
    [InlineData("ddeeffgg", "10-6-18-12")]
    public void Play_ShouldApplyActionsCorrectly(string actions, string expected)
    {
        Game game = new Game(actions);

        string result = game.Play();
        
        Assert.Equal(expected, result);
    }
}