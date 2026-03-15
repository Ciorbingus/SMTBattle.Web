using SMTBattle.Web.Enums;

namespace SMTBattle.Web.Models;

public class Demon : Troop
{
    public Race Race { get; set; }
    public string? Description { get; set; }
    
}