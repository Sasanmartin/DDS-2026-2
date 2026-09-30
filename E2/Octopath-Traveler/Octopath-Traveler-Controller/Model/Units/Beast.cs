namespace Octopath_Traveler;

public class Beast : Unit
{
    private const int BreakingPointDurationRounds = 2;

    public BeastSkill Skill { get; }
    public int Shields { get; private set; }
    public bool IsInBreakingPoint { get; private set; }

    private int MaxShields { get; }
    private IReadOnlyList<string> Weaknesses { get; }
    private int RecoversAtRound { get; set; } = -1;

    public Beast(string name, Stats stats, BeastSkill skill, int shields,
        IReadOnlyList<string> weaknesses)
        : base(name, stats)
    {
        Skill = skill;
        MaxShields = shields;
        Shields = shields;
        Weaknesses = weaknesses;
    }

    public bool IsWeakTo(AttackType type)
        => type.IsNamed && Weaknesses.Contains(type.Name);

    public void LoseShield()
    {
        if (Shields > 0)
            Shields--;
    }

    // El Breaking Point dura la ronda actual y la siguiente.
    public void EnterBreakingPoint(int round)
    {
        IsInBreakingPoint = true;
        RecoversAtRound = round + BreakingPointDurationRounds;
    }

    public void RecoverFromBreakingPoint()
    {
        IsInBreakingPoint = false;
        RecoversAtRound = -1;
        Shields = MaxShields;
    }

    public bool IsBrokenDuring(int round)
        => IsInBreakingPoint && RecoversAtRound > round;

    public bool RecoversInRound(int round)
        => RecoversAtRound == round;
}
