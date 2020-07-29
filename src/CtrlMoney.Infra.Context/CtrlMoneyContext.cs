using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Domain.Identity;
using CtrlMoney.Infra.Context.Mapping.FinancialClassification;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace CtrlMoney.Infra.Context
{
    public class CtrlMoneyContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim,
                                                          ApplicationUserRole, ApplicationUserLogin,
                                                          ApplicationRoleClaim, ApplicationUserToken>
    {

        public DbSet<Composite> Composites { get; set; }
        public DbSet<Leaf> Leaves { get; set; }
        public CtrlMoneyContext()
        {
            // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public CtrlMoneyContext(DbContextOptions<CtrlMoneyContext> options)
             : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void SetTrackAll()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationFailure>();
            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.ApplyConfiguration(new CompositeMap());
            modelBuilder.ApplyConfiguration(new LeafMap());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
                {
                // Each User can have many UserClaims
                b.HasMany(e => e.UserClaims)
                        .WithOne(e => e.User)
                        .HasForeignKey(uc => uc.UserId)
                        .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.UserLogins)
                            .WithOne(e => e.User)
                            .HasForeignKey(ul => ul.UserId)
                            .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.UserTokens)
                            .WithOne(e => e.User)
                            .HasForeignKey(ut => ut.UserId)
                            .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                            .WithOne(e => e.User)
                            .HasForeignKey(ur => ur.UserId)
                            .IsRequired();

                });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                                .WithOne(e => e.Role)
                                .HasForeignKey(rc => rc.RoleId)
                                .IsRequired();
            });
        }
    }
}
