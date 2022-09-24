using Godot;
using System;


/// <summary>
/// Singleton with several static functions
/// </summary>
public class Utility : Node
{

    /// <summary>
    /// Middle bottom of the screen
    /// </summary>
    public static Vector2 MiddleBottomCoord() {return new Vector2(960, 1080);}
    public static float MiddleX() {return 960;}
    public static int CountChar(ref string text, char character)
    {
        int output = 0;
        foreach(char c in text)
            if (c == character)
                output++;
        return output;
    }

    //Just a shorthand for loading dialogue, so I don't have to do all the casting
    public static DialogueEngine InstanceDialogue()
    {
        return GD.Load<PackedScene>("res://Scenes/DialogueEngine.tscn").Instance<DialogueEngine>();
    }

    public static BattleEngine InstanceBattle()
    {
        return GD.Load<PackedScene>("res://Scenes/BattleEngine.tscn").Instance<BattleEngine>();
    }

    // BBCode formatted names of the characters used in CharacterBar
    public static string CharacterBBName(CharacterEnum who)
    {
        switch (who)
        {
            case CharacterEnum.Player:
                return "[b]Y[/b]ou";
            case CharacterEnum.Claus:
                return "[color=#c0694b][b]C[/b][/color]laus";
            default:
                return "Utility.CharacterBBName is not fully implemented or given an incorrect argument.";
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
                return "Utility.SkillBBName does not contain " + type.ToString();
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
                return "Utility.SkillBBName does not contain " + type.ToString();
        }
    }

    public static string ATK(string text)
    {
        return "[atk][b]" + text + "[/b][/atk]";
    }
    public static string STA(string text)
    {
        return "[st][b]" + text + "[/b][/st]";
    }

    public static int BasicDamageFormula(int userATK, int targetDEF, float targetEfficiency = 1, float coefATK = 1)
    {
        int damage = Mathf.RoundToInt((userATK * coefATK - targetDEF) * targetEfficiency);
        return (damage < 1) ? 1 : damage;
    }

    public static bool IsPhysical(SkillElement type)
    {
        if (type == SkillElement.SLASH || type == SkillElement.BLUNT || type == SkillElement.PIERCE)
            return true;
        return false;
    }
    public static bool IsMagical(SkillElement type)
    {
        if (type == SkillElement.FIRE || type == SkillElement.ICE || type == SkillElement.LIGHTNING)
            return true;
        return false;
    }

    /// <summary>
    /// Takes PlayerEnum and returns its size
    /// </summary>
    public static int PlayerEnumSize(PlayerEnum type)
    {
        if (type == PlayerEnum.Body)
            return Enum.GetNames(typeof(PlayerBody)).Length;
        if (type == PlayerEnum.Head)
            return Enum.GetNames(typeof(PlayerHead)).Length;
        if (type == PlayerEnum.Eye)
            return Enum.GetNames(typeof(PlayerEye)).Length;
        if (type == PlayerEnum.Nose)
            return Enum.GetNames(typeof(PlayerNose)).Length;
        if (type == PlayerEnum.Brow)
            return Enum.GetNames(typeof(PlayerBrow)).Length;
        if (type == PlayerEnum.Hair)
            return Enum.GetNames(typeof(PlayerHair)).Length;
        if (type == PlayerEnum.Sideburn)
            return Enum.GetNames(typeof(PlayerSideburn)).Length;
        if (type == PlayerEnum.Beard)
            return Enum.GetNames(typeof(PlayerBeard)).Length;
        if (type == PlayerEnum.Moustache)
            return Enum.GetNames(typeof(PlayerMoustache)).Length;
        if (type == PlayerEnum.Scruff)
            return Enum.GetNames(typeof(PlayerScruff)).Length;

        throw new ArgumentException("PlayerEnumSize not implemented completely or given invalid type.");
    }

    /// <summary>
    /// Takes enumName and index and returns the name of the specific PlayerEnum's name with value of index
    /// </summary>
    public static string PlayerEnumName(PlayerEnum type, int index)
    {
        if (type == PlayerEnum.Body)
            return ((PlayerBody) index).ToString();
        if (type == PlayerEnum.Head)
            return ((PlayerHead) index).ToString();
        if (type == PlayerEnum.Eye)
            return ((PlayerEye) index).ToString();
        if (type == PlayerEnum.Nose)
            return ((PlayerNose) index).ToString();
        if (type == PlayerEnum.Brow)
            return ((PlayerBrow) index).ToString();
        if (type == PlayerEnum.Sideburn)
            return ((PlayerSideburn) index).ToString();
        if (type == PlayerEnum.Hair)
            return ((PlayerHair) index).ToString();
        if (type == PlayerEnum.Beard)
            return ((PlayerBeard) index).ToString();
        if (type == PlayerEnum.Moustache)
            return ((PlayerMoustache) index).ToString();
        if (type == PlayerEnum.Scruff)
            return ((PlayerScruff) index).ToString();
        
        throw new ArgumentException("PlayerEnumName not implemented completely or given invalid type.");
    }
    public static void SetShaderColor(CanvasItem sprite, PlayerSkin color)
    {
        ShaderMaterial material = (ShaderMaterial)sprite.Material;
        if (color == PlayerSkin.MID)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 1f);
            material.SetShaderParam("contrast", 1f);
            material.SetShaderParam("saturation", 1f);
        }
        else if (color == PlayerSkin.MID_YELLOW)
        {
            material.SetShaderParam("hue", 0.016f);
            material.SetShaderParam("brightness", 0.94f);
            material.SetShaderParam("contrast", 1f);
            material.SetShaderParam("saturation", 1.05f);
        }
        else if (color == PlayerSkin.PALE)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 1.15f);
            material.SetShaderParam("contrast", 1f);
            material.SetShaderParam("saturation", 1f);
        }
        else if (color == PlayerSkin.PALE_YELLOW)
        {
            material.SetShaderParam("hue", 0.016f);
            material.SetShaderParam("brightness", 1.05f);
            material.SetShaderParam("contrast", 1f);
            material.SetShaderParam("saturation", 1.05f);
        }
        else if (color == PlayerSkin.TAN)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 0.82f);
            material.SetShaderParam("contrast", 1.15f);
            material.SetShaderParam("saturation", 1f);
        }
        else if (color == PlayerSkin.TAN_YELLOW)
        {
            material.SetShaderParam("hue", 0.02f);
            material.SetShaderParam("brightness", 0.76f);
            material.SetShaderParam("contrast", 1.15f);
            material.SetShaderParam("saturation", 1.15f);
        }
        else if (color == PlayerSkin.DARK)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 0.55f);
            material.SetShaderParam("contrast", 1.2f);
            material.SetShaderParam("saturation", 1.1f);
        }
        else if (color == PlayerSkin.DARK)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 0.55f);
            material.SetShaderParam("contrast", 1.2f);
            material.SetShaderParam("saturation", 1.1f);
        }
        else if (color == PlayerSkin.DARK_YELLOW)
        {
            material.SetShaderParam("hue", 0.02f);
            material.SetShaderParam("brightness", 0.52f);
            material.SetShaderParam("contrast", 1.2f);
            material.SetShaderParam("saturation", 1.1f);
        }
        else if (color == PlayerSkin.DARK_YELLOW)
        {
            material.SetShaderParam("hue", 0.02f);
            material.SetShaderParam("brightness", 0.52f);
            material.SetShaderParam("contrast", 1.2f);
            material.SetShaderParam("saturation", 1.1f);
        }
        else if (color == PlayerSkin.DARKEST)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 0.4f);
            material.SetShaderParam("contrast", 1.2f);
            material.SetShaderParam("saturation", 1.15f);
        }
        else if (color == PlayerSkin.DARKEST_YELLOW)
        {
            material.SetShaderParam("hue", 0.02f);
            material.SetShaderParam("brightness", 0.38f);
            material.SetShaderParam("contrast", 1.22f);
            material.SetShaderParam("saturation", 1.25f);
        }
    }
    public static void SetShaderColor(CanvasItem sprite, PlayerColor color)
    {
        ShaderMaterial material = (ShaderMaterial)sprite.Material;
        if (color == PlayerColor.GRAPE)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 1f);
            material.SetShaderParam("contrast", 1f);
            material.SetShaderParam("saturation", 1f);
        }
        else if (color == PlayerColor.DARK_GRAPE)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 0.75f);
            material.SetShaderParam("contrast", 1.1f);
            material.SetShaderParam("saturation", 1f);
        }
        else if (color == PlayerColor.LIGHT_GINGER)
        {
            material.SetShaderParam("hue", 0.055f);
            material.SetShaderParam("brightness", 1.7f);
            material.SetShaderParam("contrast", 1.05f);
            material.SetShaderParam("saturation", 1.55f);
        }
        else if (color == PlayerColor.GINGER)
        {
            material.SetShaderParam("hue", 0.05f);
            material.SetShaderParam("brightness", 1f);
            material.SetShaderParam("contrast", 1f);
            material.SetShaderParam("saturation", 1.9f);
        }
        else if (color == PlayerColor.WHITE)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 3.8f);
            material.SetShaderParam("contrast", 0.8f);
            material.SetShaderParam("saturation", 0.18f);
        }
        else if (color == PlayerColor.LIGHT_GRAY)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 2.2f);
            material.SetShaderParam("contrast", 1.1f);
            material.SetShaderParam("saturation", 0.2f);
        }
        else if (color == PlayerColor.GRAY)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 1.7f);
            material.SetShaderParam("contrast", 1.1f);
            material.SetShaderParam("saturation", 0.2f);
        }
        else if (color == PlayerColor.BLACK)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 1f);
            material.SetShaderParam("contrast", 1.1f);
            material.SetShaderParam("saturation", 0.2f);
        }
        else if (color == PlayerColor.DARK_BLACK)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 0.8f);
            material.SetShaderParam("contrast", 1.2f);
            material.SetShaderParam("saturation", 0.2f);
        }
        else if (color == PlayerColor.BROWN)
        {
            material.SetShaderParam("hue", 0.05f);
            material.SetShaderParam("brightness", 0.8f);
            material.SetShaderParam("contrast", 1f);
            material.SetShaderParam("saturation", 1f);
        }
        else if (color == PlayerColor.DARK_BROWN)
        {
            material.SetShaderParam("hue", 0.05f);
            material.SetShaderParam("brightness", 0.8f);
            material.SetShaderParam("contrast", 1.15f);
            material.SetShaderParam("saturation", 0.7f);
        }
        else if (color == PlayerColor.LIGHT_BLOND)
        {
            material.SetShaderParam("hue", 0.115f);
            material.SetShaderParam("brightness", 2.1f);
            material.SetShaderParam("contrast", 1.1f);
            material.SetShaderParam("saturation", 1.15f);
        }
        else if (color == PlayerColor.BLOND)
        {
            material.SetShaderParam("hue", 0.11f);
            material.SetShaderParam("brightness", 1f);
            material.SetShaderParam("contrast", 1.02f);
            material.SetShaderParam("saturation", 1.4f);
        }
    }

    public static void SetShaderColor(CanvasItem sprite, PlayerEyeColor color)
    {
        ShaderMaterial material = (ShaderMaterial)sprite.Material;
        if (color == PlayerEyeColor.LIGHT_BLUE)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 1f);
            material.SetShaderParam("contrast", 1f);
            material.SetShaderParam("saturation", 1f);
        }
        else if (color == PlayerEyeColor.BLUE)
        {
            material.SetShaderParam("hue", 0f);
            material.SetShaderParam("brightness", 0.85f);
            material.SetShaderParam("contrast", 1.7f);
            material.SetShaderParam("saturation", 1f);
        }
        else if (color == PlayerEyeColor.LIGHT_GREEN)
        {
            material.SetShaderParam("hue", 0.54f);
            material.SetShaderParam("brightness", 0.95f);
            material.SetShaderParam("contrast", 1.2f);
            material.SetShaderParam("saturation", 0.8f);
        }
        else if (color == PlayerEyeColor.GREEN)
        {
            material.SetShaderParam("hue", 0.54f);
            material.SetShaderParam("brightness", 0.8f);
            material.SetShaderParam("contrast", 1.7f);
            material.SetShaderParam("saturation", 1f);
        }
        else if (color == PlayerEyeColor.LIGHT_BROWN)
        {
            material.SetShaderParam("hue", 0.47f);
            material.SetShaderParam("brightness", 0.8f);
            material.SetShaderParam("contrast", 1.7f);
            material.SetShaderParam("saturation", 1f);
        }
        else if (color == PlayerEyeColor.BROWN)
        {
            material.SetShaderParam("hue", 0.47f);
            material.SetShaderParam("brightness", 0.7f);
            material.SetShaderParam("contrast", 1.65f);
            material.SetShaderParam("saturation", 1f);
        }
        else if (color == PlayerEyeColor.BLACK)
        {
            material.SetShaderParam("hue", 0.44f);
            material.SetShaderParam("brightness", 0.65f);
            material.SetShaderParam("contrast", 1.8f);
            material.SetShaderParam("saturation", 0.5f);
        }
    }
}
