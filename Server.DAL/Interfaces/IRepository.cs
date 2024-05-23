using System.Linq.Expressions;

namespace Server.DAL.Interfaces;

//CRUD
public interface IRepository<T> : IDisposable where T : class
{
    void Save(); //commit
    Task SaveAsync();
    void Detach(T model);
    IQueryable<T> Read();
    IQueryable<T> Read(Expression<Func<T, bool>> expressionWhere);
    IQueryable<T> ReadTracking(Expression<Func<T, bool>> expressionWhere);
    void Create(T item);
    Task CreateAsync(T item);
    void Update(T item);
    void Delete(T item);
}