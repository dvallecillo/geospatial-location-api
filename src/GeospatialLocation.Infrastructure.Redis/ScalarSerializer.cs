using System;
using System.Text;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class ScalarSerializer : IScalarSerializer
    {
        public T? Deserialize<T>(byte[]? data)
        {
            if (data == null)
            {
                return default;
            }

            if (data.Length == 0)
            {
                return default;
            }

            var value = Encoding.UTF8.GetString(data);
            return (T?)Convert.ChangeType(value, typeof(T));
        }

        public byte[] Serialize<T>(T? data)
        {
            if (data == null)
            {
                return Array.Empty<byte>();
            }

            var value = data.ToString();
            var bytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
            return bytes;
        }
    }
}