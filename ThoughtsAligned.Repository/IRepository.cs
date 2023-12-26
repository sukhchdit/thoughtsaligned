using ThoughtsAligned.Context.Entities;
using ThoughtsAligned.Models.Dto;
using ThoughtsAligned.Models.ViewModels;
using System.Linq.Expressions;

namespace ThoughtsAligned.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);

        T? Get(Expression<Func<T, bool>> predicate);

        Task<T?> Find(long id);

        void Add(T entity);

        void AddRange(IEnumerable<T> list);

        void Update(T entity);

        void Delete(T entity);

        Task SaveChangesAsync();

        void SaveChanges();
    }
}
