using Microsoft.AspNetCore.Identity;

namespace SMTBattle.Web.Models;

public class User : IdentityUser
{
    public UserProfile? Profile { get; set; }
    public DateTime MemberSince { get; set; }

    public Hero? HeroUnit { get; set; }

    public User()
    {
        Profile = new UserProfile
        {
            DisplayName = UserName,
            Bio = ""
        };

        HeroUnit = new Hero
        {
            Id = Id,
            Name = "Hero",
            User = this,
            ProfileImageUrl = "/images/heroes/marine.webp",

            Strength = 3,
            Magic = 3,
            Vitality = 3,
            Agility = 3,
            Luck = 3,
            Level = 35
        };

    }

}