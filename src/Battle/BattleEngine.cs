using Godot;
using System.Collections.Generic;

public class BattleEngine : Node2D
{
    public List<Character> party;
    public List<Character> bench;
    public List<Monster> monsters;

    public void Initiate(MainEngine mainEngine, BattleSetup battleSetup)
    {
        party = new List<Character>();

        foreach (CharacterEnum i in mainEngine.gameData.party)
        {
            if (mainEngine.gameData.GetCharacter(i) != null)
                party.Add(mainEngine.gameData.GetCharacter(i).Clone());
        }
        
        monsters = new List<Monster>();
    }
}
