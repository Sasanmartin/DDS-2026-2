namespace Octopath_Traveler;

public class TurnQueue
{
    public List<Unit> BuildOrder(List<Traveler> travelers, List<Beast> beasts)
    {
        List<Unit> units = travelers.Cast<Unit>().Concat(beasts).ToList();
        return units
            .Select((unit, boardPosition) => (unit, boardPosition))
            .OrderByDescending(item => item.unit.Stats.Speed)
            .ThenBy(item => item.boardPosition)
            .Select(item => item.unit)
            .ToList();
    }
}
