using Microsoft.AspNetCore.Identity;

namespace SMTBattle.Web.Models;

public class User : IdentityUser
{
    public Profile? Profile { get; set; }
    public DateTime MemberSince { get; set; }

    public User()
    {
        Profile = new Profile 
        { 
            DisplayName = UserName,
            Bio = "None."
        };
    }
}