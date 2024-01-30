
using Microsoft.EntityFrameworkCore;
using ClamManagement.Data.Entities.Commen;
using ClaimManagement.Data.Entities;
using ClamManagement.Services;
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
        public DbSet<Claim> Claims { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=192.168.1.3;port=3306;user=user;password=123456;database=ClaimManagement;Convert Zero Datetime=True;");
           // optionsBuilder.UseMySQL("server=localhost;port=3306;user=Ibrahem;password=123456;database=Clam;Convert Zero Datetime=True;");

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
            var userId = 0;
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);


            base.OnModelCreating(modelBuilder);
        }
        

    }
}
