using System;
using ClaimManagement.Repo;
using ClaimManagement.Services.PolicyManagement;
using ClamManagement.Data;
using ClamManagement.Data.Entities;

namespace ClamManagement.Data
{
    public interface IUnitOfWork
    {
        IWebHostEnvironment WebHostEnvironment { get; }
        IClaimRepo ClaimRepo { get; }
        ITPARepo TPARepo { get; }
        IPolicyManagement PolicyManagement { get; }
        INetworkProviderRepo NetworkProviderRepo { get; }

        void Dispose();
        Task<int> SaveChangesAsync();

    }
    public class UnitOfWork : IUnitOfWork
    {
        private Context _db ;
        public IWebHostEnvironment WebHostEnvironment { get; }

        public IClaimRepo ClaimRepo { get; }
        public ITPARepo TPARepo { get; }
        public INetworkProviderRepo NetworkProviderRepo { get; }

        public IPolicyManagement PolicyManagement { get; }

        public UnitOfWork(Context context, IWebHostEnvironment webHostEnvironment, IClaimRepo claimRepo, ITPARepo tPARepo, IPolicyManagement policyManagement,INetworkProviderRepo networkProviderRepo)
        {
            _db = context;
            WebHostEnvironment = webHostEnvironment;
            ClaimRepo = claimRepo;
            TPARepo = tPARepo;
            this.PolicyManagement = policyManagement;
            this.NetworkProviderRepo = networkProviderRepo;
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