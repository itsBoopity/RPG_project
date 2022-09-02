using System;

public class MonsterFactory
{
    public static Monster Create(string monsterID)
    {
        switch(monsterID)
        {
            case "slime":
                return new Slime();
            default:
                throw new ArgumentException("MonsterFactory monsterID not valid or not implemented.");
        }
    }
}