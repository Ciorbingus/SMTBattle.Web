namespace SMTBattle.Web.Models;

public class Profile
{
    public string Id { get; set; }
    public string? DisplayName { get; set; }
    public string? Bio { get; set; }
    public User User { get; set; }
}