﻿
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using ClamManagement.Data.Entities;
using System.Security.Principal;
using ClamManagement.Data.Entities.Commen;
using ClaimManagement.Data.Entities;
using ClamManagement.Data.Services;
namespace ClamManagement.Data
{
    public class Context : DbContext
    {


        private readonly IUserManagement _userManagement;
        public Context() : base() { }
        public Context(DbContextOptions options , IUserManagement userManagement) : base(options) 
        {
            _userManagement = userManagement;
        }
        public DbSet<TPA> TPAs { get; set; }
        public DbSet<NetworkProvider> NetworkProviders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=192.168.1.3;port=3306;user=user;password=123456;database=Clam;Convert Zero Datetime=True;");

        }
        public override int SaveChanges()
        {
            var userId =  _userManagement.GetUserId().Result;
            foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.UpdatedAt = DateTime.Now;
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.UpdatedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.Now;
                        entry.Entity.UpdatedBy = userId;
                        break;
                }
            }
            return base.SaveChanges();
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = await _userManagement.GetUserId();
            foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.UpdatedAt = DateTime.Now;
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.UpdatedBy = userId;
                    
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.Now;
                        entry.Entity.UpdatedBy = userId;
                        break;
                }
            }
            return await base.SaveChangesAsync();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new CategoryConfigurations());
          
            
            base.OnModelCreating(modelBuilder);
        }
        

    }
}
