namespace Octopath_Traveler;

public class SkillRequest
{
    public SkillRequest(ActiveSkill skill, Unit? target, AttackType? weapon)
    {
        Skill = skill;
        Target = target;
        Weapon = weapon;
    }

    public ActiveSkill Skill { get; }
    public Unit? Target { get; }
    public AttackType? Weapon { get; }
}
