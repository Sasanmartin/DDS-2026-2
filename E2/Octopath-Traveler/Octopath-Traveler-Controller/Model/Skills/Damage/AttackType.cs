namespace Octopath_Traveler;

public sealed class AttackType
{
    public static IReadOnlyList<string> WeaponNames { get; } = new List<string>
    {
        "Sword",
        "Spear",
        "Dagger",
        "Axe",
        "Bow",
        "Stave"
    };

    private static readonly HashSet<string> PhysicalNames = new(WeaponNames, StringComparer.Ordinal);

    public static AttackType None { get; } = new("", isPhysical: false, isElemental: false);
    public static AttackType PhysicalAttack { get; } = new("", isPhysical: true, isElemental: false);
    public static AttackType ElementalAttack { get; } = new("", isPhysical: false, isElemental: true);

    private AttackType(string name, bool isPhysical, bool isElemental)
    {
        Name = name;
        IsPhysical = isPhysical;
        IsElemental = isElemental;
    }

    public string Name { get; }
    public bool IsPhysical { get; }
    public bool IsElemental { get; }
    public bool IsNamed => Name.Length > 0;

    public static AttackType FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return None;

        bool isPhysical = PhysicalNames.Contains(name);
        return new AttackType(name, isPhysical, isElemental: !isPhysical);
    }

    public IReadOnlyList<AttackType> Repeat(int count)
    {
        List<AttackType> hits = new();
        for (int i = 0; i < count; i++)
            hits.Add(this);
        return hits;
    }

    public override string ToString() => Name;
}
