using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;
using SMTBattle.Web.Models;

namespace SMTBattle.Web.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<UserProfile> UserProfiles { get; set; }

    public DbSet<Hero> Heroes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserProfile>()
            .HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<UserProfile>(p => p.Id);
    

        modelBuilder.Entity<Hero>()
            .HasKey(h => h.Id);

        modelBuilder.Entity<User>()
            .HasOne(u => u.HeroUnit)
            .WithOne(h => h.User)
            .HasForeignKey<Hero>(h => h.Id);
    
        modelBuilder.Entity<Hero>()
            .Property(h => h.PartySlots)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null!),
                v => System.Text.Json.JsonSerializer.Deserialize<TeamSlot[]>(v, (System.Text.Json.JsonSerializerOptions)null!)
        );

        modelBuilder.Entity<Hero>()
            .Property(h => h.Resistances)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null!),
                v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<SMTBattle.Web.Enums.Elements, int>>(v, (System.Text.Json.JsonSerializerOptions)null!)
        );

        modelBuilder.Entity<Hero>()
            .Property(h => h.Skills)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null!),
                v => System.Text.Json.JsonSerializer.Deserialize<SMTBattle.Web.Enums.SkillName[]>(v, (System.Text.Json.JsonSerializerOptions)null!)
        );

        modelBuilder.Entity<Hero>().Ignore(h => h.StatusEffects);
    }

    
}