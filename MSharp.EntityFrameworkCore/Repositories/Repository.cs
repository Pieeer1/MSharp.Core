using Microsoft.EntityFrameworkCore;
using MSharp.EntityFrameworkCore.Repositories.Interfaces;
using System.Linq.Expressions;

namespace MSharp.EntityFrameworkCore.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _dbContext;

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(T entity)
    {
        _dbContext.Set<T>().Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().AddRange(entities);
    }

    public IQueryable<T> GetAll()
    {
        return _dbContext.Set<T>().AsQueryable();
    }

    public T? GetSingle(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.Set<T>().FirstOrDefault(predicate);
    }

    public void Remove(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public void RemoveWhere(Expression<Func<T, bool>> predicate)
    {
        DbSet<T> setT = _dbContext.Set<T>();

        var entities = setT.Where(predicate).ToList();

        foreach (var entity in entities)
        {
            setT.Remove(entity);
        }
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}