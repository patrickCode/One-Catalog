using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Catalog.Common.Interfaces.Repository;

namespace Microsoft.Catalog.Database.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity>, IRepositoryAsync<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private dynamic AddCreatedByAndCreatedOn(dynamic entity, string createdBy = null)
        {
            if (createdBy == null)
                createdBy = "SYS";
            entity.CreatedBy = createdBy;
            entity.CreatedOn = DateTime.UtcNow;
            return entity;
        }

        private dynamic AddModifedByAndModifedOn(dynamic entity, string modifiedBy = null)
        {
            if (modifiedBy == null)
                modifiedBy = "SYS";
            entity.LastModifiedBy = modifiedBy;
            entity.LastModifiedOn = DateTime.UtcNow;
            return entity;
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

        private int GetId(dynamic entity)
        {
            return entity.Id;
        }

        public void Create(TEntity entity, string createdBy = null)
        {
            entity = AddCreatedByAndCreatedOn(entity, createdBy);
            entity = AddModifedByAndModifedOn(entity, createdBy);
            var addedEntity = _dbContext.Set<TEntity>().Add(entity);
        }

        public int CreateAndSave(TEntity entity, string createdBy = null)
        {
            entity = AddCreatedByAndCreatedOn(entity, createdBy);
            entity = AddModifedByAndModifedOn(entity, createdBy);
            var addedEntity = _dbContext.Set<TEntity>().Add(entity);
            Save();
            return GetId(entity);
        }

        public async Task<int> CreateAndSaveAsync(TEntity entity, string createdBy = null)
        {
            entity = AddCreatedByAndCreatedOn(entity, createdBy);
            return await Task.Run(async () =>
            {
                entity = AddCreatedByAndCreatedOn(entity, createdBy);
                var addedEntity = _dbContext.Set<TEntity>().Add(entity);
                await SaveAsync();
                return GetId(addedEntity.Entity);
            });
        }

        public Task CreateAsync(TEntity entity, string createdBy = null)
        {
            entity = AddCreatedByAndCreatedOn(entity, createdBy);
            return Task.Run(() =>
            {
                entity = AddCreatedByAndCreatedOn(entity, createdBy);
                var addedEntity = _dbContext.Set<TEntity>().Add(entity);
            });
        }

        public void Update(TEntity entity, string modifiedBy = null)
        {
            entity = AddModifedByAndModifedOn(entity, modifiedBy);
            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public Task UpdateAsync(TEntity entity, string modifiedBy = null)
        {
            return Task.Run(() =>
            {
                entity = AddModifedByAndModifedOn(entity, modifiedBy);
                _dbContext.Set<TEntity>().Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            });
        }

        public void Delete(object id)
        {
            var entity = _dbContext.Set<TEntity>().FirstOrDefault(obj => (GetProperty(obj, "Id") == id));
            if (entity != null)
                Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            var dbSet = _dbContext.Set<TEntity>();
            if (_dbContext.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);
            dbSet.Remove(entity);
        }

        public Task DeleteAsync(object id)
        {
            var entity = _dbContext.Set<TEntity>().FirstOrDefault(obj => (GetProperty(obj, "Id") == id));
            return DeleteAsync(entity);
        }

        public Task DeleteAsync(TEntity entity)
        {
            return Task.Run(() =>
            {
                var dbSet = _dbContext.Set<TEntity>();
                if (_dbContext.Entry(entity).State == EntityState.Detached)
                    dbSet.Attach(entity);
                dbSet.Remove(entity);
            });
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public Task SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}