using TenantSubscriptionApp.Areas.Identity.Data;
using TenantSubscriptionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace TenantSubscriptionApp.Data;

public class AuthDBContext : IdentityDbContext<ApplicationUser>
{

    public DbSet<TenantSubscription> TenantSubscriptions { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Organisation> Organisations { get; set; }
    public AuthDBContext(DbContextOptions<AuthDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
           .HasOne(u => u.Organisation)
           .WithMany(o => o.Users)
           .HasForeignKey(u => u.OrganisationId)
           .OnDelete(DeleteBehavior.Restrict);

        // Configure Organisation's relationships and constraints
        builder.Entity<Organisation>()
            .HasMany(o => o.Subscriptions)
            .WithOne(ts => ts.Organisation)
            .HasForeignKey(ts => ts.OrganisationId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: Configure delete behavior if needed

        // Configure Application's relationships and constraints
        builder.Entity<Application>()
            .HasMany(a => a.Subscriptions)
            .WithOne(ts => ts.Application)
            .HasForeignKey(ts => ts.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: Configure delete behavior if needed

        // Configure TenantSubscriptionApp's relationships and constraints
        builder.Entity<TenantSubscription>()
            .HasKey(ts => ts.Id);

        builder.Entity<TenantSubscription>()
            .HasOne(ts => ts.Organisation)
            .WithMany(o => o.Subscriptions)
            .HasForeignKey(ts => ts.OrganisationId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: Configure delete behavior if needed

        builder.Entity<TenantSubscription>()
            .HasOne(ts => ts.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(ts => ts.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: Configure delete behavior if needed

        builder.Entity<TenantSubscription>()
            .HasOne(ts => ts.Application)
            .WithMany(a => a.Subscriptions)
            .HasForeignKey(ts => ts.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: Configure delete behavior if needed

        //builder.Entity<ApplicationUser>()
        //   .HasMany(t => t.Subscriptions)
        //   .WithOne(ts => ts.User)
        //   .HasForeignKey(ts => ts.UserId)
        //   .IsRequired()
        //   .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<TenantSubscription>().Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Entity<Organisation>().HasIndex(o => o.Name).IsUnique();

        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
    }
}
