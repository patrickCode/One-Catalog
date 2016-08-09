using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Microsoft.Catalog.Common.Interfaces.Repository
{
    public interface IReadOnlyRepository<TEntity> where TEntity: class
    {
        IEnumerable<TEntity> GetAll(
            Func<TEntity, object> orderBy = null,
            List<string> inculdedProperties = null,
            int? skip = null,
            int? top = null);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<TEntity, object> orderBy = null,
            List<string> inculdedProperties = null,
            int? skip = null,
            int? top = null);

        TEntity Get(object id);

        bool Exists(Expression<Func<TEntity, bool>> filter);

        int GetCount(Expression<Func<TEntity, bool>> filter = null);
    }
}