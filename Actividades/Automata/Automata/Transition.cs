namespace Automata;

public readonly record struct Transition(int StartingState, char Symbol, int EndingState);
