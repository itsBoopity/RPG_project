using System.Collections.Generic;

public partial class MonsterCodex
{
    private static Dictionary<int,int> codex = new Dictionary<int,int>();

    public static void Analyze(int monsterId)
    {
        if (!codex.ContainsKey(monsterId))
        {
            codex.Add(monsterId, 1);
            return;
        }

        if (codex[monsterId] < 100) //Only keep it up to 100 to prevent overflow. Who the fuck would do it 2 million times!?
            codex[monsterId] += 1;
    }

    public int ViewCodex(int monsterId)
    {
        return codex[monsterId];
    }

}