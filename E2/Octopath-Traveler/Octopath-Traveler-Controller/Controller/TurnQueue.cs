namespace Octopath_Traveler;

public class TurnQueue
{
    public List<Unit> BuildOrder(IReadOnlyList<Traveler> travelers, IReadOnlyList<Beast> beasts, int round)
    {
        // Categorías de prioridad: recuperar Breaking Point > Defender > prioridad+ > normal > prioridad−.
        List<Unit> board = travelers.Cast<Unit>().Concat(beasts).Where(unit => unit.IsAlive).ToList();
        List<Unit> available = board.Where(unit => !IsBrokenDuring(unit, round)).ToList();
        List<Unit> ordered = new();
        Add(ordered, TravelerFirst(available.Where(unit => IsRecovering(unit, round)), board));
        Add(ordered, TravelerFirst(available.Where(unit => unit.DefendedInRound(round - 1)), board));
        Add(ordered, TravelerFirst(available.Where(unit => unit.HasIncreasedPriorityIn(round)), board));
        Add(ordered, SpeedFirst(available.Where(unit => !unit.HasDecreasedPriorityIn(round)), board));
        Add(ordered, TravelerFirst(available.Where(unit => unit.HasDecreasedPriorityIn(round)), board));
        return ordered;
    }

    private static void Add(List<Unit> ordered, IEnumerable<Unit> category)
        => ordered.AddRange(category.Where(unit => !ordered.Contains(unit)));

    private static IEnumerable<Unit> TravelerFirst(IEnumerable<Unit> units, List<Unit> board)
        => units
            .OrderByDescending(unit => unit is Traveler)
            .ThenByDescending(unit => unit.Stats.Speed)
            .ThenBy(unit => board.IndexOf(unit));

    private static IEnumerable<Unit> SpeedFirst(IEnumerable<Unit> units, List<Unit> board)
        => units
            .OrderByDescending(unit => unit.Stats.Speed)
            .ThenByDescending(unit => unit is Traveler)
            .ThenBy(unit => board.IndexOf(unit));

    private static bool IsRecovering(Unit unit, int round)
        => unit is Beast beast && beast.RecoversInRound(round);

    private static bool IsBrokenDuring(Unit unit, int round)
        => unit is Beast beast && beast.IsBrokenDuring(round);
}
