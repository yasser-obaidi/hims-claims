using System;
using ClamManagement.Data;
using ClamManagement.Data.Entities;

namespace ClamManagement.Repo
{
    public interface IUnitOfWork
    {
        IWebHostEnvironment WebHostEnvironment { get; }
        void Dispose();
        Task<int> SaveChangesAsync();

    }
    public class UnitOfWork : IUnitOfWork
    {
        private Context _db ;
        public IWebHostEnvironment WebHostEnvironment { get; }


        public UnitOfWork(Context context, IWebHostEnvironment webHostEnvironment)
        {
            _db = context;
            WebHostEnvironment = webHostEnvironment;
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