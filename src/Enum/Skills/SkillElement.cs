using Godot;

public enum SkillElement
{
    NONE,
    /// <summary>
    /// Element of skill depends on weapon equipped.
    /// </summary>
    VARIABLE,
    SLASH,
    PIERCE,
    BLUNT,
    FIRE,
    ICE,
    LIGHTNING
}

public static class SkillElementUtility
{
    /// <summary>
    /// Returns at random SLASH, PIERCE, BLUNT, FIRE, ICE or LIGHTNING.
    /// </summary>
    public static SkillElement GetRandomPhysicalOrMagical()
    {
        switch (GD.Randi() % 6)
        {
            case 0: return SkillElement.SLASH;
            case 1: return SkillElement.PIERCE;
            case 2: return SkillElement.BLUNT;
            case 3: return SkillElement.FIRE;
            case 4: return SkillElement.ICE;
            case 5: return SkillElement.LIGHTNING;
            default: return SkillElement.NONE;
        }
    }

    /// <summary>
    /// Returns at random SLASH, PIERCE, BLUNT.
    /// </summary>
    public static SkillElement GetRandomPhysical()
    {
        switch (GD.Randi() % 3)
        {
            case 0: return SkillElement.SLASH;
            case 1: return SkillElement.PIERCE;
            case 2: return SkillElement.BLUNT;
            default: return SkillElement.NONE;
        }
    }


    /// <summary>
    /// Returns at random FIRE, ICE or LIGHTNING.
    /// </summary>
    public static SkillElement GetRandomMagical()
    {
        switch (GD.Randi() % 3)
        {
            case 0: return SkillElement.FIRE;
            case 1: return SkillElement.ICE;
            case 2: return SkillElement.LIGHTNING;
            default: return SkillElement.NONE;
        }
    }

    public static bool IsPhysical(this SkillElement type)
    {
        return type == SkillElement.SLASH || type == SkillElement.BLUNT || type == SkillElement.PIERCE;
    }
    public static bool IsMagical(this SkillElement type)
    {
        return type == SkillElement.FIRE || type == SkillElement.ICE || type == SkillElement.LIGHTNING;
    }
}