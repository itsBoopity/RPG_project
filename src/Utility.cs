using Godot;
using System;


/// <summary>
/// Singleton with several static functions
/// </summary>
public partial class Utility : Node
{
    public static int CountChar(ref string text, char character)
    {
        int output = 0;
        foreach(char c in text)
            if (c == character)
                output++;
        return output;
    }

    public static BattleEngine InstanceBattle()
    {
        return GD.Load<PackedScene>("res://Scenes/BattleEngine.tscn").Instantiate<BattleEngine>();
    }

    public static Color GetCharacterColor(string name)
    {
        switch (name)
        {
            case "Claus":
                return new Color("#c0694b");
            default:
                return new Color("#756361");

        }
    }

    // BBCode formatted names of the characters used in CharacterBar
    public static string CharacterBBName(CharacterEnum who)
    {
        switch (who)
        {
            case CharacterEnum.CLAUS:
                return "[color=#c0694b][b]C[/b][/color]laus";
            default:
                throw new ArgumentException("Utility.CharacterBBName is not fully implemented or given an incorrect argument.");
        }
    }

    // BBCode formatted skill type
    public static string SkillBBName(SkillType type)
    {
        switch (type)
        {
            case SkillType.BASIC:
                return "[color=#94705D]Basic[/color]";
            case SkillType.OFFENSIVE:
                return "[color=#DC5750]Offensive[/color]";
            default:
                throw new ArgumentException("Utility.SkillBBName does not contain " + type.ToString());
        }
    }
    
    public static string SkillBBName(SkillElement type)
    {
        switch (type)
        {
            case SkillElement.BLUNT:
                return "[color=#979EDF]Blunt[/color]";
            case SkillElement.SLASH:
                return "[color=#C86B36]Slash[/color]";
            case SkillElement.PIERCE:
                return "[color=#B868A3]Pierce[/color]";
            case SkillElement.FIRE:
                return "[color=#FF4849]Fire[/color]";
            case SkillElement.ICE:
                return "[color=#3AB7F9]Ice[/color]";
            case SkillElement.LIGHTNING:
                return "[color=#FFF936]Lightning[/color]";
            default:
                throw new ArgumentException("Utility.SkillBBName does not contain " + type.ToString());
        }
    }

    public static string ATK(string text) { return "[atk][b]" + text + "[/b][/atk]"; }
    public static string STA(string text) { return "[st][b]" + text + "[/b][/st]"; }

    public static int BasicDamageFormula(int userATK, int targetDEF, float targetEfficiency = 1, float coefATK = 1)
    {
        int damage = Mathf.RoundToInt((userATK * coefATK - targetDEF) * targetEfficiency);
        return (damage < 1) ? 1 : damage;
    }

    public static bool IsPhysical(SkillElement type)
    {
        return type == SkillElement.SLASH || type == SkillElement.BLUNT || type == SkillElement.PIERCE;
    }
    public static bool IsMagical(SkillElement type)
    {
        return type == SkillElement.FIRE || type == SkillElement.ICE || type == SkillElement.LIGHTNING;
    }

    public static void SetHslShader(ShaderMaterial shader, float hue = 0.0f, float brightness = 1.0f, float contrast = 1.0f, float saturation = 1.0f)
    {
        shader.SetShaderParameter("hue", hue);
        shader.SetShaderParameter("brightness", brightness);
        shader.SetShaderParameter("contrast", contrast);
        shader.SetShaderParameter("saturation", saturation);
    }
}
