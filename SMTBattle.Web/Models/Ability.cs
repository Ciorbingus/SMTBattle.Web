namespace SMTBattle.Web.Models;

public class Ability
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public int Type { get; set; }
}