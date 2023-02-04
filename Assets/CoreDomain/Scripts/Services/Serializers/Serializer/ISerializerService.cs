namespace Handlers.Serializers.Serializer
{
    public interface ISerializerService
    {
        string SerializeJson<T>(T obj);
        T DeserializeJson<T>(string json);

        string SerializeCsv<T>(T[] obj);
        T[] DeserializeCsv<T>(string csv);

        void PopulateJson(string json, object target);
    }
}