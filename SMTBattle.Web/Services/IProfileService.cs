using SMTBattle.Web.Models;

namespace SMTBattle.Web.Services;

public interface IProfileService
{
    Task<UserProfile?> GetProfileByIdAsync(string userId);
    Task<UserProfile?> GetProfileByUsernameAsync(string username);
    Task<bool> UpdateProfileAsync(UserProfile profile);
    Task<string> SaveAvatarAsync(string userId, Stream fileStream, string fileName);
    Task<bool> DeleteProfileAsync(string userId);
}