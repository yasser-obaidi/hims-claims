using System;
using ClaimManagement.Data.Repo;
using ClamManagement.Data;
using ClamManagement.Data.Entities;

namespace ClamManagement.Data
{
    public interface IUnitOfWork
    {
        IWebHostEnvironment WebHostEnvironment { get; }
        IClaimRepo ClaimRepo { get; }
        ITPARepo TPARepo { get; }
        void Dispose();
        Task<int> SaveChangesAsync();

    }
    public class UnitOfWork : IUnitOfWork
    {
        private Context _db ;
        public IWebHostEnvironment WebHostEnvironment { get; }

        public IClaimRepo ClaimRepo { get; }
        public ITPARepo TPARepo { get; }

        public UnitOfWork(Context context, IWebHostEnvironment webHostEnvironment, IClaimRepo claimRepo, ITPARepo tPARepo)
        {
            _db = context;
            WebHostEnvironment = webHostEnvironment;
            ClaimRepo = claimRepo;
            TPARepo = tPARepo;
        }
        public async Task<int> SaveChangesAsync()
        {
           return await _db.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}