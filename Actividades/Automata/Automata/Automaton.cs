namespace Automata;

public class Automaton
{
    private const int InitialState = 0;

    private readonly HashSet<int> _acceptingStates;
    private readonly Dictionary<(int State, char Symbol), int> _transitions;

    public Automaton(AutomatonDefinition definition)
    {
        _acceptingStates = definition.AcceptingStates;
        _transitions = definition.Transitions.ToDictionary(
            transition => (transition.StartingState, transition.Symbol),
            transition => transition.EndingState);
    }

    public bool Accepts(string word)
        => _acceptingStates.Contains(Run(word));

    private int Run(string word)
    {
        int state = InitialState;
        foreach (char symbol in word)
            state = Move(state, symbol);

        return state;
    }

    private int Move(int state, char symbol)
        => _transitions.TryGetValue((state, symbol), out int nextState) ? nextState : state;
}
