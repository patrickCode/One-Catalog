using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Catalog.Database.Models;
using Microsoft.Catalog.Common.Interfaces.Repository;

namespace Microsoft.Catalog.Database.Repositories
{
    public class BaseReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>, IReadOnlyRepositoryAsync<TEntity> where TEntity: BaseModel
    {
        protected DbContext _dbContext;
        public BaseReadOnlyRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected virtual IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<TEntity, object> orderBy = null, 
            List<string> inculdedProperties = null, int? skip = default(int?), int? top = default(int?)
            )
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            if (filter != null)
                query = query.Where(filter);

            if (inculdedProperties != null)
            {
                inculdedProperties.ForEach(property =>
                {
                    query = query.Include(obj => GetProperty(obj, property));
                });
            }

            if (orderBy != null)
                query = query.OrderBy(orderBy).AsQueryable();

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (top.HasValue)
                query = query.Take(top.Value);

            return query;
        }

        private object GetProperty(TEntity obj, string propertyName)
        {
            Type objType = obj.GetType();
            List<PropertyInfo> props = objType.GetProperties().ToList();
            var prop = props.FirstOrDefault(p => p.Name.Equals(propertyName));
            if (prop != null)
                return prop.GetValue(obj, null);
            return null;
        }

        public bool Exists(Expression<Func<TEntity, bool>> filter)
        {
            return Query(filter).Any();
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            return Query(filter).AnyAsync();
        }

        public virtual TEntity Get(object id)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(obj => obj.Id == (int)id);
        }

        public virtual Task<TEntity> GetAsync(object id)
        {
            return _dbContext.Set<TEntity>().FirstOrDefaultAsync(obj => obj.Id == (int)id);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<TEntity, object> orderBy = null,
            List<string> inculdedProperties = null,
            int? skip = default(int?),
            int? top = default(int?))
        {
            return await Query(filter, orderBy, inculdedProperties, skip, top).ToListAsync();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<TEntity, object> orderBy = null,
            List<string> inculdedProperties = null, 
            int? skip = default(int?), 
            int? top = default(int?))
        {
            return Query(filter, orderBy, inculdedProperties, skip, top).ToList();
        }

        public virtual IEnumerable<TEntity> GetAll(Func<TEntity, object> orderBy = null,
            List<string> inculdedProperties = null, 
            int? skip = default(int?), 
            int? top = default(int?))
        {
            return Query(null, orderBy, inculdedProperties, skip, top).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Func<TEntity, object> orderBy = null,
            List<string> inculdedProperties = null, 
            int? skip = default(int?), 
            int? top = default(int?))
        {
            return await Query(null, orderBy, inculdedProperties, skip, top).ToListAsync();
        }

        public int GetCount(Expression<Func<TEntity, bool>> filter)
        {
            return Query(filter, null, null, null, null).Count();
        }

        public virtual Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return Query(filter, null, null, null, null).CountAsync();
        }
    }
}