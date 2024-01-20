using Microsoft.EntityFrameworkCore;
using ClaimManagement.Data.Entities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using ClamManagement.Data;

namespace ClaimManagement.Data.Repo
{
    public interface IClaimRepo 
    {
        Task<Claim> FindById(int id);
        Task<IQueryable<Claim >> FindByCondition(Expression<Func<Claim, bool>> expression);
        void Add(Claim entity);
        void AddRange(IEnumerable<Claim> entity);
        void DeleteRange(IEnumerable<Claim> entity);
        Task<bool> Any(Expression<Func<Claim, bool>> expression);
        Task<bool> All(Expression<Func<Claim, bool>> expression);
    }
    public class ClaimRepo : IClaimRepo
    {
        private readonly Context _db;
        public ClaimRepo(Context db)
        {
            _db = db;
        }
        public async Task<Claim> FindById(int id)
        {
            return await _db.Claims.FindAsync(id);
        }
        public async Task<IQueryable<Claim>> FindByCondition(Expression<Func<Claim, bool>> expression)
        {
            return _db.Claims.Where(expression).OrderByDescending(t => t.CreatedAt);

        }
        public async Task<bool> Any(Expression<Func<Claim, bool>> expression)
        {
            return _db.Claims.Any(expression);

        }
        public async Task<bool> All(Expression<Func<Claim, bool>> expression)
        {
            return _db.Claims.All(expression);

        }
        public void Add(Claim entity)
        {

            _db.Claims.Add(entity);
        }
        public void AddRange(IEnumerable<Claim> entity)
        {
            _db.Claims.AddRange(entity);
        }
        public void Delete(Claim entity)
        {

            _db.Claims.Remove(entity);
        }
        public void DeleteRange(IEnumerable<Claim> entity)
        {
            _db.Claims.RemoveRange(entity);
        }
    }
}
