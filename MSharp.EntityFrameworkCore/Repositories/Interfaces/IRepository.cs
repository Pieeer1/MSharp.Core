using System.Linq.Expressions;

namespace MSharp.EntityFrameworkCore.Repositories.Interfaces;
public interface IRepository<T> where T : class
{
    public T? GetSingle(Expression<Func<T, bool>> predicate);
    public IQueryable<T> GetAll();
    public void Add(T entity);
    public void AddRange(IEnumerable<T> entities);
    public void Remove(T entity);
    public void RemoveWhere(Expression<Func<T, bool>> predicate);
    public void SaveChanges();
}