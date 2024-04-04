using System.Collections.Generic;
using System.Text.Json;
public class CharacterStatsExporter : IJsonExporter<CharacterStats>
{
    public string Export(CharacterStats data)
    {
        List<SkillId> skills = new();
        foreach (BattleSkillData skill in data.Skills)
        {
            skills.Add(skill.Id);
        }

        return JsonSerializer.Serialize(new {
            data.Who,
            data.Name,
            data.Level,
            data.Health,
            data.MaxHealth,
            data.Strength,
            data.Intelligence,
            data.Defense,
            data.Speed,
            data.Element,
            data.ElementalAffinity,
            skills
        });
    }
}