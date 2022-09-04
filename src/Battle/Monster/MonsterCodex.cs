using System.Collections.Generic;

public class MonsterCodex
{
    private static Dictionary<int,int> codex = new Dictionary<int,int>();

    public static void Analyze(int monsterID)
    {
        if (!codex.ContainsKey(monsterID))
        {
            codex.Add(monsterID, 1);
            return;
        }

        if (codex[monsterID] < 100) //Only keep it up to 100 to prevent overflow. Who the fuck would do it 2 million times!?
            codex[monsterID] += 1;
    }

    public int ViewCodex(int monsterID)
    {
        return codex[monsterID];
    }

}