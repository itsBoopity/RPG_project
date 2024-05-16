using Godot;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public partial class GameData : Node
{
    private static GameData _instance;

    [JsonProperty]
    private CharacterStats csYellam;
    [JsonProperty]
    private CharacterStats csSrinivas;
    [JsonProperty]
    private CharacterStats csIshke;
    [JsonProperty]
    private CharacterStats csFray;
    [JsonProperty]
    private List<CharacterEnum> party;
    [JsonProperty]
    private List<CharacterEnum> bench;

    public static GameData Instance => _instance;

    public override void _EnterTree()
    {
        if (_instance != null) this.QueueFree();
        else _instance = this;
    }

    // TODO: currently required to initialize data when launching directly into dungeon engine scene for texting,
    // remove after adding a proper start menu
    public override void _Ready()
    {
        NewSave();
    }

    /// <summary>
    /// Returns the corresponding CharacterData.
    /// </summary>
    public CharacterStats GetCharacter(CharacterEnum characterEnum)
    {
        return characterEnum switch
        {
            CharacterEnum.YELLAM => csYellam,
            CharacterEnum.SRINIVAS => csSrinivas,
            CharacterEnum.ISHKE => csIshke,
            CharacterEnum.FRAY => csFray,
            _ => throw new ArgumentException($"GameData::GetCharacter does not recognize enum: {characterEnum}"),
        };

    }
    /// <summary>
    /// Updates CharacterStats health.
    /// </summary>
    public void UpdateCharacterHealth(CharacterEnum who, int health)
    {
        GetCharacter(who).Health = health;
    }

    /// <summary>
    /// Creates a list of BattleCharacters in the main party
    /// </summary>
    public List<BattleCharacter> GetBattleParty()
    {
        List<BattleCharacter> output = new();
        foreach (CharacterEnum who in party)
        {
            if (who != CharacterEnum.NULL)
            {
                output.Add(new BattleCharacter(GetCharacter(who)));
            }
        }
        return output;
    }

    /// <summary>
    /// Creates a list of BattleCharacters in the bench
    /// </summary>
    public List<BattleCharacter> GetBattleBench()
    {
        List<BattleCharacter> output = new();
        foreach (CharacterEnum who in bench)
        {
            output.Add(new BattleCharacter(GetCharacter(who)));
        }
        return output;
    }

    /// <summary>
    /// Updates the order of characters in party and bench.
    /// </summary>
    /// <param name="newParty">Ordered list of characters in party.</param>
    /// <param name="newBench">Ordered list of characters in bench.</param>
    public void UpdatePartyLoadout(List<BattleCharacter> newParty, List<BattleCharacter> newBench)
    {
        party.Clear();
        bench.Clear();
        foreach (BattleCharacter character in newParty)
        {
            party.Add(character.Who);
        }
        foreach (BattleCharacter character in newBench)
        {
            bench.Add(character.Who);
        }
    }

    public static void Load(GameData data)
    {
        _instance = data;
    }

    public void NewSave() // Create New Save
    {
        csYellam = GD.Load<CharacterStats>("res://Resources/Stats/Character/Yellam.tres");
        csSrinivas = GD.Load<CharacterStats>("res://Resources/Stats/Character/Srinivas.tres");
        csIshke = GD.Load<CharacterStats>("res://Resources/Stats/Character/Ishke.tres");
        csFray = GD.Load<CharacterStats>("res://Resources/Stats/Character/Fray.tres");
        party = new List<CharacterEnum>{CharacterEnum.YELLAM, CharacterEnum.FRAY, CharacterEnum.ISHKE};
        bench = new List<CharacterEnum>{CharacterEnum.SRINIVAS};
    }
}
