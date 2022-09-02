using Godot;

public class Slime: Monster
{
    public Slime()
    {
        name = "Slime";
        monsterID = "slime";
        maxHP = 10;
        HP = 10;
        ATK = 2;
        DEF = 1;
    }

    public override void LoadUpcomingTurn(ref BattleEngine battleEngine)
    {
        targetCharacter = 0;
    }

    
}