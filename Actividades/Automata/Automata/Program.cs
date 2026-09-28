// See https://aka.ms/new-console-template for more information

using Automata;

string automatonFilePath = Path.Combine(AppContext.BaseDirectory, "DFAs", "02.txt");
string word = "aaabcbb";

Automaton automaton = new AutomatonFactory().CreateFromFile(automatonFilePath);

if (automaton.Accepts(word))
    Console.WriteLine("Palabra aceptada");
else
    Console.WriteLine("Palabra rechazada");
