using System;

public partial class MonsterFactory
{
    public static BattleMonster Create(MonsterId monsterId)
    {
        switch(monsterId)
        {
            case MonsterId.Slime: return new BattleSlime();
            default:
                throw new ArgumentException("MonsterId " + monsterId + "wasn't added to the MonsterFactory.");
        }
        
    }

    public static string GetName(MonsterId monsterId)
    {
        // TODO: ew, why is slime name stored/duplicated here
        if (monsterId == MonsterId.Slime) return "Slime";

        throw new ArgumentException("MonsterId " + monsterId + "wasn't added to the MonsterFactory.");
    }
}