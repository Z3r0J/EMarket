using EMarket.Core.Domain.Common;
using EMarket.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMarket.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) :base(options){}

        public DbSet<User> Users { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<Advertising> Advertisings { get; set; }
        public DbSet<Category> Categories { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            //Fluent API

            #region Tables

            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<Gallery>()
                .ToTable("Gallery");

            modelBuilder.Entity<Advertising>()
                .ToTable("Advertisings");

            modelBuilder.Entity<Category>()
                .ToTable("Categories");

            #endregion

            #region Primary Keys

            modelBuilder.Entity<User>()
                .HasKey(user=>user.Id);

            modelBuilder.Entity<Gallery>()
                .HasKey(gallery => gallery.Id);

            modelBuilder.Entity<Advertising>()
                .HasKey(advertising=>advertising.Id);

            modelBuilder.Entity<Category>()
                .HasKey(category => category.Id);

            #endregion

            #region RelationShips

            modelBuilder.Entity<User>()
                .HasMany(user => user.Advertisings)
                .WithOne(advertising => advertising.User)
                .HasForeignKey(advertising=>advertising.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Advertising>()
                .HasMany(advertising => advertising.Gallery)
                .WithOne(gallery => gallery.Advertisings)
                .HasForeignKey(gallery=>gallery.AdvertisingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasMany(category => category.Advertisings)
                .WithOne(advertising => advertising.Categories)
                .HasForeignKey(advertising => advertising.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Properties Configuration

            #region Users

            modelBuilder.Entity<User>()
                .Property(user => user.Name)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(user => user.LastName)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(user => user.Phone)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(user => user.Password)
                .IsRequired();
            #endregion

            #region Advertising

            modelBuilder.Entity<Advertising>()
                .Property(advertising => advertising.Name)
                .IsRequired();
            
            modelBuilder.Entity<Advertising>()
                .Property(advertising => advertising.Description)
                .IsRequired();
            
            modelBuilder.Entity<Advertising>()
                .Property(advertising => advertising.Price)
                .IsRequired();

            #endregion
            
            #region Category

            modelBuilder.Entity<Category>()
                .Property(category => category.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            modelBuilder.Entity<Category>()
                .Property(category => category.Description)
                .IsRequired();

            #endregion

            #endregion

        }
    }
}
