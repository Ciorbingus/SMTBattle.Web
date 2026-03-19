namespace SMTBattle.Web.Services;

using SMTBattle.Web.Data;
using SMTBattle.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;


public class ProfileService : IProfileService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ProfileService(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<UserProfile?> GetProfileByIdAsync(string userId)
    {
        return await _context.Set<UserProfile>()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == userId);
    }

    public async Task<UserProfile?> GetProfileByUsernameAsync(string username)
    {
        return await _context.Set<UserProfile>()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.User.UserName == username);
    }

    public async Task<bool> UpdateProfileAsync(UserProfile profile)
    {
        var existingProfile = await _context.UserProfiles.FindAsync(profile.Id);
        if (existingProfile == null) return false;

        if (!string.IsNullOrEmpty(existingProfile.ProfileImageUrl) &&
        existingProfile.ProfileImageUrl != profile.ProfileImageUrl)
        {
            DeletePhysicalFile(existingProfile.ProfileImageUrl);
        }

        existingProfile.DisplayName = profile.DisplayName;
        existingProfile.Bio = profile.Bio;
        existingProfile.ProfileImageUrl = profile.ProfileImageUrl;

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<string> SaveAvatarAsync(string userId, Stream fileStream, string fileName)
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(fileStreamOutput);
        }

        return $"/uploads/{uniqueFileName}";
    }

    public async Task<bool> DeleteProfileAsync(string userId)
    {
        var profile = await _context.UserProfiles.FindAsync(userId);
        if (profile == null) return false;

        if (!string.IsNullOrEmpty(profile.ProfileImageUrl))
        {
            DeletePhysicalFile(profile.ProfileImageUrl);
        }

        _context.UserProfiles.Remove(profile);
        return await _context.SaveChangesAsync() > 0;
    }

    private void DeletePhysicalFile(string relativePath)
    {
        try
        {
            if (relativePath.Contains("default-avatar")) return;

            var filePath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine($"[SYSTEM] Purged file: {filePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Could not delete file: {ex.Message}");
        }
    }

}