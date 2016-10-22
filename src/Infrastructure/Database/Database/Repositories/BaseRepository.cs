using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Catalog.Database.Models;
using Microsoft.Catalog.Common.Interfaces.Repository;

namespace Microsoft.Catalog.Database.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity>, IRepositoryAsync<TEntity> where TEntity : BaseModel
    {
        protected readonly DbContext _dbContext;
        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private TEntity AddCreatedByAndCreatedOn(TEntity entity, string createdBy = null)
        {
            if (string.IsNullOrEmpty(createdBy))
                createdBy = string.IsNullOrEmpty(entity.CreatedBy) ? "SYS" : entity.CreatedBy;
            entity.CreatedBy = createdBy;
            entity.CreatedOn = DateTime.UtcNow;
            return entity;
        }

        private TEntity AddModifedByAndModifedOn(TEntity entity, string modifiedBy = null)
        {
            if (string.IsNullOrEmpty(modifiedBy))
                modifiedBy = string.IsNullOrEmpty(entity.LastModifiedBy) ? "SYS" : entity.LastModifiedBy;
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
            return entity.Id;
        }

        public async Task<int> CreateAndSaveAsync(TEntity entity, string createdBy = null)
        {
            entity = AddCreatedByAndCreatedOn(entity, createdBy);
            return await Task.Run(async () =>
            {
                entity = AddCreatedByAndCreatedOn(entity, createdBy);
                var addedEntity = _dbContext.Set<TEntity>().Add(entity);
                await SaveAsync();
                return addedEntity.Entity.Id;
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
            var attachedEntity = _dbContext.ChangeTracker.Entries<TEntity>()
                .FirstOrDefault(e => e.Entity.Id == entity.Id);
            if (attachedEntity != null)
                _dbContext.Entry<TEntity>(attachedEntity.Entity).State = EntityState.Detached;
            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public Task UpdateAsync(TEntity entity, string modifiedBy = null)
        {
            return Task.Run(() =>
            {
                entity = AddModifedByAndModifedOn(entity, modifiedBy);
                var attachedEntity = _dbContext.ChangeTracker.Entries<TEntity>()
                    .FirstOrDefault(e => e.Entity.Id == entity.Id);
                if (attachedEntity != null)
                    _dbContext.Entry<TEntity>(attachedEntity.Entity).State = EntityState.Detached;
                _dbContext.Set<TEntity>().Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            });
        }

        public void Delete(object id, bool softDelete = false)
        {
            var entity = _dbContext.Set<TEntity>().FirstOrDefault(obj => obj.Id == (int)id);
            if (entity != null)
                Delete(entity, softDelete);
        }

        public void Delete(TEntity entity, bool softDelete = false)
        {
            if (!softDelete)
            {
                var dbSet = _dbContext.Set<TEntity>();
                if (_dbContext.Entry(entity).State == EntityState.Detached)
                    dbSet.Attach(entity);
                dbSet.Remove(entity);
            }
            else
            {
                entity.IsDeleted = true;
                Update(entity);
            }
        }

        public Task DeleteAsync(object id, bool softDelete = false)
        {
            var entity = _dbContext.Set<TEntity>().FirstOrDefault(obj => obj.Id == (int)id);
            return DeleteAsync(entity, softDelete);
        }

        public Task DeleteAsync(TEntity entity, bool softDelete = false)
        {
            if (!softDelete)
            {
                return Task.Run(() =>
                {

                    var dbSet = _dbContext.Set<TEntity>();
                    if (_dbContext.Entry(entity).State == EntityState.Detached)
                        dbSet.Attach(entity);
                    dbSet.Remove(entity);

                });
            }
            else
            {
                entity.IsDeleted = true;
                return UpdateAsync(entity);
            }
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