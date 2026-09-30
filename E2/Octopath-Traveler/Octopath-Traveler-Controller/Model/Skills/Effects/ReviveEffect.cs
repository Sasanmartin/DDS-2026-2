namespace Octopath_Traveler;

public class ReviveEffect : ISkillEffect
{
    public IReadOnlyList<SkillOutcome> Apply(SkillContext context, IReadOnlyList<Unit> targets)
    {
        List<SkillOutcome> outcomes = new();
        foreach (Unit target in targets)
        {
            if (target.IsAlive)
                continue;
            target.Revive();
            outcomes.Add(new ReviveOutcome(target));
        }
        return outcomes;
    }
}
