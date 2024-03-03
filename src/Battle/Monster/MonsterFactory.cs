using System;

public partial class MonsterFactory
{
    public static Monster Create(MonsterId monsterId)
    {
        switch(monsterId)
        {
            case MonsterId.Slime: return new Slime();
            default:
                throw new ArgumentException("MonsterId " + monsterId + "wasn't added to the MonsterFactory.");
        }
        
    }

    public static string GetName(MonsterId monsterId)
    {
        if (monsterId == Slime.GetId()) return "Slime";

        throw new ArgumentException("MonsterId " + monsterId + "wasn't added to the MonsterFactory.");
    }
}