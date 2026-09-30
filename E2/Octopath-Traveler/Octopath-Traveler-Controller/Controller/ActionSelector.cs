using Octopath_Traveler_View;

namespace Octopath_Traveler;

public class ActionSelector
{
    private readonly View _view;
    private readonly CombatState _state;
    private readonly IReadOnlyList<Beast> _beasts;
    private readonly CombatReporter _reporter;

    public ActionSelector(View view, CombatState state, CombatReporter reporter)
    {
        _view = view;
        _state = state;
        _beasts = state.Beasts;
        _reporter = reporter;
    }

    public int SelectAction(string travelerName)
        => _view.SelectAction(travelerName);

    public BasicAttackRequest? SelectBasicAttack(Traveler traveler)
    {
        string? weapon = SelectWeapon(traveler);
        if (weapon is null) return null;
        Beast? target = SelectBeastTarget(traveler);
        if (target is null) return null;
        ShowBoostMenu(traveler);
        return new BasicAttackRequest(weapon, target);
    }

    public SkillRequest? SelectSkillRequest(Traveler traveler)
    {
        ActiveSkill? skill = SelectSkill(traveler);
        if (skill is null) return null;
        AttackType? weapon = SelectSkillAttackType(skill);
        Unit? target = SelectSkillTarget(traveler, skill);
        if (skill.RequiresWeaponChoice && weapon is null) return null;
        if (skill.RequiresTargetChoice && target is null) return null;
        ShowBoostMenu(traveler);
        return new SkillRequest(skill, target, weapon);
    }

    private ActiveSkill? SelectSkill(Traveler traveler)
    {
        List<ActiveSkill> usableSkills = traveler.ActiveSkills
            .Where(skill => traveler.HasEnoughSp(skill.SpCost))
            .ToList();
        int choice = _view.SelectSkill(traveler.Name, usableSkills.Select(skill => skill.Name).ToList());
        return choice == usableSkills.Count + 1 ? null : usableSkills[choice - 1];
    }

    private string? SelectWeapon(Traveler traveler)
    {
        int choice = _view.SelectWeapon(traveler.Weapons);
        return choice == traveler.Weapons.Count + 1 ? null : traveler.Weapons[choice - 1];
    }

    private Beast? SelectBeastTarget(Traveler traveler)
    {
        List<Beast> aliveBeasts = _beasts.Where(beast => beast.IsAlive).ToList();
        int choice = _view.SelectTarget(traveler.Name, _reporter.ToTargetDisplays(aliveBeasts));
        return choice == aliveBeasts.Count + 1 ? null : aliveBeasts[choice - 1];
    }

    private Unit? SelectTargetFor(Traveler traveler, ActiveSkill skill)
    {
        IReadOnlyList<Unit> candidates = skill.GetCandidateTargets(new SkillContext(traveler, _state));
        int choice = _view.SelectTarget(traveler.Name, _reporter.ToTargetDisplays(candidates));
        return choice == candidates.Count + 1 ? null : candidates[choice - 1];
    }

    private void ShowBoostMenu(Traveler traveler)
    {
        if (traveler.BoostPoints < 1)
            return;

        _view.AskBoostPoints();
    }

    private AttackType? SelectAttackType()
    {
        IReadOnlyList<string> weaponNames = AttackType.WeaponNames;
        int choice = _view.SelectWeapon(weaponNames);
        return choice == weaponNames.Count + 1 ? null : AttackType.FromName(weaponNames[choice - 1]);
    }

    private AttackType? SelectSkillAttackType(ActiveSkill skill)
        => skill.RequiresWeaponChoice ? SelectAttackType() : null;

    private Unit? SelectSkillTarget(Traveler traveler, ActiveSkill skill)
        => skill.RequiresTargetChoice ? SelectTargetFor(traveler, skill) : null;
}
