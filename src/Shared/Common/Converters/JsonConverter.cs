using Newtonsoft.Json;
using Microsoft.Catalog.Common.Exceptions;

namespace Microsoft.Catalog.Common.Converters
{
    public class JsonConverter<T> : IConverter<T> where T: class
    {
        public T Deserialize(string serializedObj, bool allowNull = false)
        {
            if (typeof(T) == typeof(string))
                return serializedObj as T;
            var currentObject = JsonConvert.DeserializeObject<T>(serializedObj);
            if (currentObject == null && !allowNull)
                throw new GeneralException(
                    Constants.ExceptionMessages.Application.UnableToConvert, 
                    (int)Constants.ExceptionCodes.Application.UnableToConvert);
            return currentObject;
         }

        public string Serialize(T obj)
        {
            if (typeof(T) == typeof(string))
                return obj as string;
            return JsonConvert.SerializeObject(obj);
        }
    }
}