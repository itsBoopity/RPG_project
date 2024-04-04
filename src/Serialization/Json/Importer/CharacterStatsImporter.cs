using Godot;
using System.Text.Json;
public class CharacterStatsImporter : IJsonImporter<CharacterStats>
{
	// You need to use a different method, because this requires reflection...
    public CharacterStats Import(string json)
    {
		// dynamic data = JsonConverter.Deserialize<dynamic>(json);
		// GD.Print(data);
		// JsonConvert

		// GD.Print(data.who);

        // CharacterStats output = new()
        // {
        //     who = data.who,
        //     Name = data.Name,
        //     Level = data.Level,
        //     Health = data.Health,
        //     MaxHealth = data.MaxHealth,
        //     Strength = data.Strength,
        //     Intelligence = data.Intelligence,
        //     Defense = data.Defense,
        //     Speed = data.Speed,
        //     Element = data.Element,
        //     elementalAffinity = data.elementalAffinity
        // };

        return null;
    }
}