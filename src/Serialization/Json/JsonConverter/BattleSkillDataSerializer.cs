using System;
using Godot;
using Newtonsoft.Json;
public class BattleSkillDataSerializer : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BattleSkillData);
    }

    public override BattleSkillData ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return GD.Load<BattleSkillData>((string)reader.Value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(((BattleSkillData)value).ResourcePath);
    }
}