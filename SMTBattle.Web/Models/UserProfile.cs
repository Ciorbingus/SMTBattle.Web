namespace SMTBattle.Web.Models;

public class UserProfile
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? Bio { get; set; }
    public User? User { get; set; }

    public string? ProfileImageUrl { get; set; } = "/images/default-avatar.jpg";
}