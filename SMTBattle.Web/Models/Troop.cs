namespace SMTBattle.Web.Models;

using SMTBattle.Web.Enums;

public abstract class Troop
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "Dummy";
    public string ProfileImageUrl { get; set; } = "/images/demons/dummy.png";
    public int Level { get; set; } = 35;
    public Allignment Allignment { get; set; }

    public int Strength { get; set; } = 1;
    public int Magic { get; set; } = 1;
    public int Vitality { get; set; } = 1;
    public int Agility { get; set; } = 1;
    public int Luck { get; set; } = 1;

    public int HP { get; set; } = 1;
    public int MaxHP { get; set; } = 1; 
    public int MP { get; set; } = 1;
    public int MaxMP { get; set; } = 1;

    public bool IsDead => HP <= 0;

    public Dictionary<Elements, int> Resistances { get; set; } = new();
    public Dictionary<StatusEffect, int> StatusEffects { get; set; } = new();
    public SkillName[] Skills { get; set; } = new SkillName[8];

    protected Troop()
    {
        foreach (StatusEffect effect in Enum.GetValues(typeof(StatusEffect)))
        {
            StatusEffects[effect] = 0;
        }

        foreach (Elements element in Enum.GetValues(typeof(Elements)))
        {
            Resistances[element] = 0; 
        }

        for (int i = 0; i < Skills.Length; i++)
        {
            Skills[i] = SkillName.None;
        }
    }
}