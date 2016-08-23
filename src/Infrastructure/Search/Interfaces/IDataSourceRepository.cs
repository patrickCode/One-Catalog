using System.Collections.Generic;
using Microsoft.Azure.Search.Models;

namespace Microsoft.Catalog.Azure.Search.Interface
{
    public interface IDataSourceRepository
    {
        DataSource Get(string dataSourceName);
        List<DataSource> Get();
        bool Exists(string dataSourceName);
        void Create(string dataSourceName, string dataSourceJsonStr, bool force = false);
        void Create(DataSource dataSource, bool force);
        void Update(string dataSourceName, string dataSourceJsonStr, bool createIfNotPresent = true);
        void Delete(string dataSourceName);
    }
}