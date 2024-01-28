using ClaimManagement.Data.Entities;
using ClamManagement.Data;
using System.Linq.Expressions;

namespace ClaimManagement.Repo;
public interface INetworkProviderRepo
{
    Task<NetworkProvider> FindById(int id);
    Task<IQueryable<NetworkProvider>> FindByCondition(Expression<Func<NetworkProvider, bool>> expression);
    void Add(NetworkProvider entity);
    void AddRange(IEnumerable<NetworkProvider> entity);
    void DeleteRange(IEnumerable<NetworkProvider> entity);
    Task<bool> Any(Expression<Func<NetworkProvider, bool>> expression);
    Task<bool> All(Expression<Func<NetworkProvider, bool>> expression);
}
public class NetworkProviderRepo : INetworkProviderRepo
{
    private readonly Context _db;
    public NetworkProviderRepo(Context db)
    {
        _db = db;
    }
    public async Task<NetworkProvider> FindById(int id)
    {
        return await _db.NetworkProviders.FindAsync(id);
    }
    public async Task<IQueryable<NetworkProvider>> FindByCondition(Expression<Func<NetworkProvider, bool>> expression)
    {
        return _db.NetworkProviders.Where(expression).OrderByDescending(t => t.CreatedAt);

    }
    public async Task<bool> Any(Expression<Func<NetworkProvider, bool>> expression)
    {
        return _db.NetworkProviders.Any(expression);

    }
    public async Task<bool> All(Expression<Func<NetworkProvider, bool>> expression)
    {
        return _db.NetworkProviders.All(expression);

    }
    public void Add(NetworkProvider entity)
    {

        _db.NetworkProviders.Add(entity);
    }
    public void AddRange(IEnumerable<NetworkProvider> entity)
    {
        _db.NetworkProviders.AddRange(entity);
    }
    public void Delete(NetworkProvider entity)
    {

        _db.NetworkProviders.Remove(entity);
    }
    public void DeleteRange(IEnumerable<NetworkProvider> entity)
    {
        _db.NetworkProviders.RemoveRange(entity);
    }
}
