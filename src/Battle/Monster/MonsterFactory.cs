using System;

public partial class MonsterFactory
{
    public static Monster Create(int monsterID)
    {
        if (monsterID == Slime.GetID()) return new Slime();

        throw new ArgumentException("MonsterID " + monsterID + "wasn't added to the MonsterFactory.");
    }

    public static string GetName(int monsterID)
    {
        if (monsterID == Slime.GetID()) return "Slime";

        throw new ArgumentException("MonsterID " + monsterID + "wasn't added to the MonsterFactory.");
    }
}