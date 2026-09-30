namespace Octopath_Traveler;

public class Stats
{
    public int HP { get; set; }
    public int SP { get; set; }
    public int PhysAtk { get; set; }
    public int PhysDef { get; set; }
    public int ElemAtk { get; set; }
    public int ElemDef { get; set; }
    public int Speed { get; set; }

    public Stats Copy()
        => new()
        {
            HP = HP,
            SP = SP,
            PhysAtk = PhysAtk,
            PhysDef = PhysDef,
            ElemAtk = ElemAtk,
            ElemDef = ElemDef,
            Speed = Speed
        };
}
