using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BulkyBooky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            // includeProperties će biti korišten za eager loading, korisno kad imamo FK reference.
            string includeProperties = null
            );
        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            // includeProperties će biti korišten za eager loading, korisno kad imamo FK reference.
            string includeProperties = null
            );

        void Add(T entity);
        void Remove(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
