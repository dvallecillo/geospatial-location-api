using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class JsonNetSerializer : ISerializer
    {
        private readonly JsonSerializerOptions _options = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public T? Deserialize<T>(byte[]? data)
        {
            return data == null ? default : JsonSerializer.Deserialize<T>(data, _options);
        }

        public byte[] Serialize<T>(T? data)
        {
            if (data == null)
            {
                return Array.Empty<byte>();
            }

            var item = JsonSerializer.Serialize(data, _options);
            return Encoding.UTF8.GetBytes(item);
        }
    }
}