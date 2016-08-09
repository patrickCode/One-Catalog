using System.Threading.Tasks;

namespace Microsoft.Catalog.Common.Interfaces.Repository
{
    public interface IRepositoryAsync<TEntity>
    {
        Task CreateAsync(TEntity entity, string createdBy = null);
        Task<int> CreateAndSaveAsync(TEntity entity, string createdBy = null);
        Task UpdateAsync(TEntity entity, string modifiedBy = null);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(object id);
        Task SaveAsync();
    }
}