using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EntityContext : DbContext
    {

        public EntityContext(DbContextOptions<EntityContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-H0M8STK\\MSSQL;database=CaseDB; integrated security=true;");
        }
        */
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedUser)
                .WithOne()
                .HasForeignKey<User>(u => u.CreatedUserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.LastUpdatedUser)
                .WithOne()
                .HasForeignKey<User>(u => u.LastUpdatedUserId);

            
            //modelBuilder.Entity<User>() 
            //    .HasMany(u => u.CreateUsers)
            //    .WithOne()
            //    .HasForeignKey(u => u.CreatedUserId);
            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.UpdateUsers)
            //    .WithOne()
            //    .HasForeignKey(u => u.LastUpdatedUserId);
        }

    }

}
