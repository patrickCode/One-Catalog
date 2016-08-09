using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Microsoft.Catalog.Common.Interfaces.Repository
{
    public interface IReadOnlyRepositoryAsync<TEntity> where TEntity: class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(
            Func<TEntity, object> orderBy = null,
            List<string> inculdedProperties = null,
            int? skip = null,
            int? top = null);

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<TEntity, object> orderBy = null,
            List<string> inculdedProperties = null,
            int? skip = null,
            int? top = null);

        Task<TEntity> GetAsync(object id);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);
    }
}