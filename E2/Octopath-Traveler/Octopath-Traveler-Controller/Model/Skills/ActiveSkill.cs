namespace Octopath_Traveler;

public class ActiveSkill
{
    private readonly ITargetSelector _targetSelector;
    private readonly IReadOnlyList<ISkillEffect> _effects;

    public ActiveSkill(string name, int spCost, ITargetSelector targetSelector,
        IReadOnlyList<ISkillEffect> effects)
    {
        Name = name;
        SpCost = spCost;
        _targetSelector = targetSelector;
        _effects = effects;
    }

    public string Name { get; }
    public int SpCost { get; }

    public bool RequiresTargetChoice => _targetSelector is IChoosableTargetSelector;
    public bool RequiresWeaponChoice => _effects.Any(effect => effect.RequiresWeaponChoice);

    public IReadOnlyList<Unit> GetCandidateTargets(SkillContext context)
        => _targetSelector is IChoosableTargetSelector choosable
            ? choosable.CandidateTargets(context)
            : Array.Empty<Unit>();

    public IReadOnlyList<SkillOutcome> Execute(SkillContext context)
    {
        IReadOnlyList<Unit> targets = _targetSelector.SelectTargets(context);
        List<SkillOutcome> outcomes = new();
        foreach (ISkillEffect effect in _effects)
            outcomes.AddRange(effect.Apply(context, targets));
        return outcomes;
    }
}
