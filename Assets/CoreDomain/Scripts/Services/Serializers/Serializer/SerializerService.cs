using System;
using Services.SerializerService;
using Services.SerializerService.Serializers.Csv;
using Services.SerializerService.Serializers.NewSoftJson;

namespace Handlers.Serializers.Serializer
{
    public class SerializerService : ISerializerService
    {
        private readonly CsvSerializer _csvSerializer = new();
        private readonly ISerializer<string> _jsonSerializer = new NewSoftJsonSerializer();

        public string SerializeJson<T>(T obj)
        {
            return _jsonSerializer.Serialize(obj);
        }

        public T DeserializeJson<T>(string json)
        {
            return _jsonSerializer.Deserialize<T>(json);
        }

        public string SerializeCsv<T>(T[] obj)
        {
            throw new NotImplementedException();
        }

        public T[] DeserializeCsv<T>(string csv)
        {
            return _csvSerializer.DeserializeCsv<T>(csv);
        }

        public void PopulateJson(string json, object target)
        {
            _jsonSerializer.Populate(json, target);
        }
    }
}