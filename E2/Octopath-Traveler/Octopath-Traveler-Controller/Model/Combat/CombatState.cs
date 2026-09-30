namespace Octopath_Traveler;

public class CombatState
{
    public CombatState(IReadOnlyList<Traveler> travelers, IReadOnlyList<Beast> beasts)
    {
        Travelers = travelers;
        Beasts = beasts;
    }

    public IReadOnlyList<Traveler> Travelers { get; }
    public IReadOnlyList<Beast> Beasts { get; }
    public int Round { get; set; }

    public bool AreAllBeastsDead => Beasts.All(beast => !beast.IsAlive);
    public bool AreAllTravelersDead => Travelers.All(traveler => !traveler.IsAlive);

    public IReadOnlyList<Unit> OpponentsOf(Unit unit)
        => unit is Traveler ? Beasts.Cast<Unit>().ToList() : Travelers.Cast<Unit>().ToList();

    public IReadOnlyList<Unit> AlliesOf(Unit unit)
        => unit is Traveler ? Travelers.Cast<Unit>().ToList() : Beasts.Cast<Unit>().ToList();

    public IReadOnlyList<Unit> LivingOpponentsOf(Unit unit)
        => OpponentsOf(unit).Where(opponent => opponent.IsAlive).ToList();

    public IReadOnlyList<Unit> LivingAlliesOf(Unit unit)
        => AlliesOf(unit).Where(ally => ally.IsAlive).ToList();

    public IReadOnlyList<Unit> FallenAlliesOf(Unit unit)
        => AlliesOf(unit).Where(ally => !ally.IsAlive).ToList();
}
