namespace Microsoft.Catalog.Common.Interfaces.Repository
{
    public interface IRepository<TEntity>
    {
        void Create(TEntity entity, string createdBy = null);
        int CreateAndSave(TEntity entity, string createdBy = null);
        void Update(TEntity entity, string modifiedBy = null);
        void Delete(TEntity entity, bool softDelete = false);
        void Delete(object id, bool softDelete = false);
        void Save();
    }
}