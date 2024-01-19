using System;
using ClamManagement.Data;
using ClamManagement.Data.Entities;

namespace ClamManagement.Repo
{
    public interface IUnitOfWork
    {
        void Dispose();
        Task<int> SaveChangesAsync();

    }
    public class UnitOfWork : IUnitOfWork
    {
        private Context _db ;


        public UnitOfWork(Context context)
        {
            _db = context;
           

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