using Microsoft.EntityFrameworkCore;
using ClaimManagement.Data.Entities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using ClamManagement.Data;

namespace ClaimManagement.Repo
{
    public interface ITPARepo 
    {
        Task<TPA> FindById(int id);
        Task<IQueryable<TPA >> FindByCondition(Expression<Func<TPA, bool>> expression);
        void Add(TPA entity);
        void AddRange(IEnumerable<TPA> entity);
        void DeleteRange(IEnumerable<TPA> entity);
        Task<bool> Any(Expression<Func<TPA, bool>> expression);
        Task<bool> All(Expression<Func<TPA, bool>> expression);
    }
    public class TPARepo : ITPARepo
    {
        private readonly Context _db;
        public TPARepo(Context db)
        {
            _db = db;
        }
        public async Task<TPA> FindById(int id)
        {
            return await _db.TPAs.FindAsync(id);
        }
        public async Task<IQueryable<TPA>> FindByCondition(Expression<Func<TPA, bool>> expression)
        {
            return _db.TPAs.Where(expression).OrderByDescending(t => t.CreatedAt);

        }
        public async Task<bool> Any(Expression<Func<TPA, bool>> expression)
        {
            return _db.TPAs.Any(expression);

        }
        public async Task<bool> All(Expression<Func<TPA, bool>> expression)
        {
            return _db.TPAs.All(expression);

        }
        public void Add(TPA entity)
        {

            _db.TPAs.Add(entity);
        }
        public void AddRange(IEnumerable<TPA> entity)
        {
            _db.TPAs.AddRange(entity);
        }
        public void Delete(TPA entity)
        {

            _db.TPAs.Remove(entity);
        }
        public void DeleteRange(IEnumerable<TPA> entity)
        {
            _db.TPAs.RemoveRange(entity);
        }
    }
}
