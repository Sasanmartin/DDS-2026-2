namespace Octopath_Traveler;

internal sealed class UnitStatus
{
    private const int NoRound = -1;
    private const int RoundsPerApplication = 2;

    public bool IsDefending { get; private set; }
    public int DefendedRound { get; private set; } = NoRound;
    public int IncreasedPriorityRound { get; private set; } = NoRound;
    public int DecreasedPriorityUntilRound { get; private set; } = NoRound;

    public void BeginDefending(int round)
    {
        IsDefending = true;
        DefendedRound = round;
    }

    public void StopDefending()
        => IsDefending = false;

    public void IncreasePriorityForRound(int round)
        => IncreasedPriorityRound = round;

    public void DecreasePriorityFrom(int currentRound)
        => DecreasedPriorityUntilRound =
            Math.Max(DecreasedPriorityUntilRound + RoundsPerApplication, currentRound + 1);

    public bool DefendedInRound(int round)
        => DefendedRound == round;

    public bool HasIncreasedPriorityIn(int round)
        => IncreasedPriorityRound == round;

    public bool HasDecreasedPriorityIn(int round)
        => DecreasedPriorityUntilRound >= round;
}
