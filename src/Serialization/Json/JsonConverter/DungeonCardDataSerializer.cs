using System;
using Godot;
using Newtonsoft.Json;
public class DungeonCardDataSerializer : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DungeonCard);
    }

    public override DungeonCard ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return GD.Load<DungeonCard>((string)reader.Value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(((DungeonCard)value).ResourcePath);
    }
}