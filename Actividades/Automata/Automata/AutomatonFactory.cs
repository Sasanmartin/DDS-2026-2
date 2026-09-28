namespace Automata;

public class AutomatonFactory
{
    private readonly AutomatonFileDecoder _decoder;

    public AutomatonFactory()
        : this(new AutomatonFileDecoder())
    {
    }

    public AutomatonFactory(AutomatonFileDecoder decoder)
        => _decoder = decoder;

    public Automaton CreateFromFile(string filePath)
        => new(_decoder.Decode(filePath));
}
