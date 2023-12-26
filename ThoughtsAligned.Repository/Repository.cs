using ThoughtsAligned.Context.Entities;
using ThoughtsAligned.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ThoughtsAligned.Context;
using ThoughtsAligned.Models.ViewModels;

namespace ThoughtsAligned.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ThoughtsAlignedContext currentContext;

        public Repository(ThoughtsAlignedContext _currentContext)
        {
            currentContext = _currentContext;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            try
            {
                if (predicate != null)
                    return currentContext.Set<T>().Where(predicate);
                else
                    return currentContext.Set<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T? Get(Expression<Func<T, bool>> predicate)
        {
            try
            {
                if (predicate != null)
                    return currentContext.Set<T>().Where(predicate).FirstOrDefault();
                else
                    return currentContext.Set<T>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T?> Find(long id)
        {
            try
            {
                return await currentContext.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Add(T entity)
        {
            try
            {
                currentContext.Set<T>().AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(T entity)
        {
            try
            {
                currentContext.Set<T>().Update(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                currentContext.Set<T>().Remove(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddRange(IEnumerable<T> list)
        {
            try
            {
                currentContext.Set<T>().AddRange(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await currentContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveChanges()
        {
            try
            {
                currentContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
