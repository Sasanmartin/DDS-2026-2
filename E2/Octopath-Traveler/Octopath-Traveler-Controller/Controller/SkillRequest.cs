namespace Octopath_Traveler;

public class SkillRequest
{
    public required ActiveSkill Skill { get; init; }
    public Unit? Target { get; init; }
    public AttackType? Weapon { get; init; }
}
