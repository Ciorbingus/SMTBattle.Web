using Microsoft.EntityFrameworkCore;
using SMTBattle.Web.Data;
using SMTBattle.Web.Models;

namespace SMTBattle.Web.Services;

public class HeroService
{
    private readonly ApplicationDbContext _context;

    public HeroService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Hero?> GetHeroWithProfileAsync(string userId)
    {
        return await _context.Heroes
            .Include(h => h.User)
            .ThenInclude(u => u!.Profile)
            .FirstOrDefaultAsync(h => h.Id == userId);
    }

    public async Task<bool> SaveHeroAsync(Hero hero)
    {
        try
        {
            _context.Heroes.Update(hero);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving hero: {ex.Message}");
            return false;
        }
    }
    
    public void RefreshDerivedStats(Hero hero)
    {
        hero.MaxHP = (hero.Vitality + hero.Level) * 6;
        hero.MaxMP = (hero.Magic + hero.Level) * 3;
        
        hero.HP = hero.MaxHP;
        hero.MP = hero.MaxMP;
    }

    public async Task<Hero> GetOrCreateHeroAsync(string userId)
{
    var hero = await _context.Heroes.FirstOrDefaultAsync(h => h.Id == userId);
    
    if (hero == null)
    {
        hero = new Hero
        {
            Id = userId,
            Name = "Hero",
            ProfileImageUrl = "/images/heroes/marine.webp",
            Strength = 3, Magic = 3, Vitality = 3, Agility = 3, Luck = 3,
            Level = 35,
        };
        
        _context.Heroes.Add(hero);
        await _context.SaveChangesAsync();
    }
    
    return hero;
}
}