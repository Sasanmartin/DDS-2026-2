namespace Octopath_Traveler;

public abstract class OpponentStatSelector : ITargetSelector
{
    protected abstract int StatOf(Unit unit);
    protected virtual bool HighestFirst => true;

    public IReadOnlyList<Unit> SelectTargets(SkillContext context)
    {
        IReadOnlyList<Unit> opponents = context.State.LivingOpponentsOf(context.User);
        if (opponents.Count == 0)
            return opponents;

        IEnumerable<Unit> ordered = HighestFirst
            ? opponents.OrderByDescending(StatOf)
            : opponents.OrderBy(StatOf);
        return new[] { ordered.First() };
    }
}

public class HighestHpOpponentSelector : OpponentStatSelector
{
    protected override int StatOf(Unit unit) => unit.CurrentHp;
}

public class HighestPhysAtkOpponentSelector : OpponentStatSelector
{
    protected override int StatOf(Unit unit) => unit.Stats.PhysAtk;
}

public class HighestElemAtkOpponentSelector : OpponentStatSelector
{
    protected override int StatOf(Unit unit) => unit.Stats.ElemAtk;
}

public class HighestSpeedOpponentSelector : OpponentStatSelector
{
    protected override int StatOf(Unit unit) => unit.Stats.Speed;
}

public class LowestPhysDefOpponentSelector : OpponentStatSelector
{
    protected override bool HighestFirst => false;
    protected override int StatOf(Unit unit) => unit.Stats.PhysDef;
}

public class LowestElemDefOpponentSelector : OpponentStatSelector
{
    protected override bool HighestFirst => false;
    protected override int StatOf(Unit unit) => unit.Stats.ElemDef;
}

public class LowestSpeedOpponentSelector : OpponentStatSelector
{
    protected override bool HighestFirst => false;
    protected override int StatOf(Unit unit) => unit.Stats.Speed;
}
