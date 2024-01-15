
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using ClamManagement.Data.Entities;
using System.Security.Principal;
using ClamManagement.Data.Entities.Commen;
using ClaimManagement.Data.Entities;
namespace ClamManagement.Data
{
    public class Context : DbContext
    {
        


        public Context() : base() { }
        public Context(DbContextOptions options) : base(options) { }
        public DbSet<TPA> TPAs { get; set; }
        public DbSet<NetworkProvider> NetworkProviders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=192.168.1.3;port=3306;user=user;password=123456;database=Clam;Convert Zero Datetime=True;");

        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.UpdatedAt = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.Now;
                        break;
                }
            }
            return base.SaveChanges();
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.UpdatedAt = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.Now;
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
