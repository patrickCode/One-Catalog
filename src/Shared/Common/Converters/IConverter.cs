namespace Microsoft.Catalog.Common.Converters
{
    public interface IConverter<T>
    {
        T Deserialize(string serializedObj, bool allowNull = false);
        string Serialize(T obj);
    }
}