namespace Automata;

public class AutomatonDefinition
{
    public AutomatonDefinition(HashSet<int> acceptingStates, IReadOnlyList<Transition> transitions)
    {
        AcceptingStates = acceptingStates;
        Transitions = transitions;
    }

    public HashSet<int> AcceptingStates { get; }

    public IReadOnlyList<Transition> Transitions { get; }
}
