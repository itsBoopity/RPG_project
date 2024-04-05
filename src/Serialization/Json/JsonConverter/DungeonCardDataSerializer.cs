using System;
using Godot;
using Newtonsoft.Json;
public class DungeonCardDataSerializer : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DungeonCardData);
    }

    public override DungeonCardData ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return GD.Load<DungeonCardData>((string)reader.Value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(((DungeonCardData)value).ResourcePath);
    }
}