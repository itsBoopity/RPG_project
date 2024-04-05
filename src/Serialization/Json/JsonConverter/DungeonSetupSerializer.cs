using System;
using Godot;
using Newtonsoft.Json;
public class DungeonSetupSerializer : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DungeonDeck);
    }

    public override DungeonSetup ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return GD.Load<DungeonSetup>((string)reader.Value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(((DungeonSetup)value).ResourcePath);
    }
}