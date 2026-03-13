using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;
using SMTBattle.Web.Models;

namespace SMTBattle.Web.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<UserProfile> UserProfiles { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserProfile>()
            .HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<UserProfile>(p => p.Id);
    }
}