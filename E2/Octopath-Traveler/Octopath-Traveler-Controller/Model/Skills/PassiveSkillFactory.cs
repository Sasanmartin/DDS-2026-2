namespace Octopath_Traveler;

public class PassiveSkillFactory
{
    private readonly Dictionary<string, Func<PassiveSkill>> _creators = new()
    {
        ["Summon Strength"] = () => new SummonStrengthPassive(),
        ["Elemental Augmentation"] = () => new ElementalAugmentationPassive(),
        ["Hale and Hearty"] = () => new HaleAndHeartyPassive(),
        ["Fleefoot"] = () => new FleefootPassive(),
        ["Inner Strength"] = () => new InnerStrengthPassive(),
        ["Vim and Vigor"] = () => new VimAndVigorPassive(),
        ["Second Wind"] = () => new SecondWindPassive(),
        ["Patience"] = () => new PatiencePassive(),
        ["Boost Start"] = () => new BoostStartPassive(),
        ["Stat Swap"] = () => new StatSwapPassive()
    };

    public PassiveSkill Create(string name)
        => _creators.TryGetValue(name, out Func<PassiveSkill>? creator)
            ? creator()
            : new NullPassive(name);
}
