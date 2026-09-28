namespace Automata;

public class AutomatonFileDecoder
{
    private const int AcceptingStatesLineIndex = 0;
    private const int FirstTransitionLineIndex = 1;
    private const int StartingStateComponentIndex = 0;
    private const int SymbolComponentIndex = 1;
    private const int EndingStateComponentIndex = 2;
    private const char ComponentSeparator = ',';

    public AutomatonDefinition Decode(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        return ParseLines(lines);
    }

    private AutomatonDefinition ParseLines(string[] lines)
    {
        HashSet<int> acceptingStates = ParseAcceptingStates(lines[AcceptingStatesLineIndex]);
        IReadOnlyList<Transition> transitions = lines
            .Skip(FirstTransitionLineIndex)
            .Select(ParseTransition)
            .ToList();

        return new AutomatonDefinition(acceptingStates, transitions);
    }

    private HashSet<int> ParseAcceptingStates(string line)
        => line.Split(ComponentSeparator).Select(int.Parse).ToHashSet();

    private Transition ParseTransition(string line)
    {
        string[] components = line.Split(ComponentSeparator);
        return new Transition(
            StartingState: int.Parse(components[StartingStateComponentIndex]),
            Symbol: char.Parse(components[SymbolComponentIndex]),
            EndingState: int.Parse(components[EndingStateComponentIndex]));
    }
}
