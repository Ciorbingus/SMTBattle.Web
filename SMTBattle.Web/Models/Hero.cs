namespace SMTBattle.Web.Models;

using SMTBattle.Web.Enums;
using System.Text.Json.Serialization;
public class Hero : Troop
{
    virtual public User? User { get; set; }

    public TeamSlot[] PartySlots { get; set; } = new TeamSlot[6];

    public Hero() : base()
    {
        for (int i = 0; i < 6; i++)
            PartySlots[i] = new TeamSlot();
    }

    public void SwitchDemon(int activeIndex, int reserveIndex)
    {
        if (activeIndex >= 0 && activeIndex <= 2 && reserveIndex >= 3 && reserveIndex <= 5)
        {
            var temp = PartySlots[activeIndex];
            PartySlots[activeIndex] = PartySlots[reserveIndex];
            PartySlots[reserveIndex] = temp;
        }
    }
}

public class TeamSlot
{
    public string? DemonId { get; set; }
    public SkillName[] CustomSkills { get; set; } = new SkillName[3];

    [JsonIgnore] public Demon? CachedDemon { get; set; }
}


