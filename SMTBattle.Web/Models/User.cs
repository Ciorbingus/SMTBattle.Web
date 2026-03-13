using Microsoft.AspNetCore.Identity;

namespace SMTBattle.Web.Models;

public class User : IdentityUser
{
    public UserProfile? Profile { get; set; }
    public DateTime MemberSince { get; set; }

    public User()
    {
        Profile = new UserProfile 
        { 
            DisplayName = UserName,
            Bio = "None."
        };
    }
}