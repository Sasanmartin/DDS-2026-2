namespace Octopath_Traveler;

public class PassiveSkillFactory
{
    private readonly Dictionary<string, Func<PassiveSkill>> _creators = new()
    {
        ["Summon Strength"] = () => new SummonStrengthPassive(),
        ["Elemental Augmentation"] = () => new ElementalAugmentationPassive(),
        ["Hale and Hearty"] = () => new HaleAndHeartyPassive(),
        ["Fleefoot"] = () => new FleefootPassive(),
        ["Inner Strength"] = () => new InnerStrengthPassive()
    };

    public PassiveSkill Create(string name)
        => _creators.TryGetValue(name, out Func<PassiveSkill>? creator)
            ? creator()
            : new NullPassive(name);
}
