using Godot;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text.Json;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Linq;


// Save File Structure:
// Version Number = helps with conversion of old files
// currentScene = to see if the player is in dungeon, in the middle of dialogue, or in the village (maybe not all this)
// data based on currentScene (dungeon state / dialogue file name and line being parsed)
// CharacterMembers in order
// Party
// bench
// Flags
public partial class GameData : Node
{
    private static GameData _instance;
    private CharacterStats csYellam;
    private CharacterStats csSrinivas;
    private CharacterStats csIshke;
    private CharacterStats csFray;
    private List<CharacterEnum> party;
    private List<CharacterEnum> bench;

    public static GameData Instance => _instance;

    public override void _EnterTree()
    {
        if (_instance != null) this.QueueFree();
        _instance = this;
    }

    // TODO: currently required to initialize data when launching directly into dungeon engine scene for texting,
    // remove after adding a proper start menu
    public override void _Ready()
    {
        newSave();
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
    
    public void Save(string filePath)
    {
        Directory.CreateDirectory(ProjectSettings.GlobalizePath("user://save"));
        FileStream file = File.Open(ProjectSettings.GlobalizePath("user://save/" + filePath), FileMode.OpenOrCreate);

        // string oneElement = JsonSerializer.Serialize<CharacterData>(cdClaus)

        //BinaryFormatter is obsolete and dangerous, use something else instead
        // bF.Serialize(file, "v0.0.1");
        // bF.Serialize(file, avaData);
        // bF.Serialize(file,playerCharacter);
        // bF.Serialize(file,clausCharacter);
        // bF.Serialize(file,party);
         



        file.Close();
    }
    public void Load(string filePath) //filePath is passed to this function through the slot selection
    {
        BinaryFormatter bF = new BinaryFormatter();
        FileStream file = File.Open(Godot.ProjectSettings.GlobalizePath("user://save/" + filePath), FileMode.Open);

        //BinaryFormatter is obsolete and dangerous, use something else instead
        // string saveVersion = (string) bF.Deserialize(file);
        // playerCharacter = (Character) bF.Deserialize(file);
        // clausCharacter = (Character) bF.Deserialize(file);
        // party = (CharacterEnum[]) bF.Deserialize(file);
        //flags = (byte[]) bF.Deserialize(file);

        file.Close();
    }

    public void newSave() // Create New Save
    {
        csYellam = GD.Load<CharacterStats>("res://Resources/Stats/Character/Yellam.tres");
        csSrinivas = GD.Load<CharacterStats>("res://Resources/Stats/Character/Srinivas.tres");
        csIshke = GD.Load<CharacterStats>("res://Resources/Stats/Character/Ishke.tres");
        csFray = GD.Load<CharacterStats>("res://Resources/Stats/Character/Fray.tres");
        party = new List<CharacterEnum>{CharacterEnum.YELLAM, CharacterEnum.FRAY, CharacterEnum.ISHKE};
        bench = new List<CharacterEnum>{CharacterEnum.SRINIVAS};
        // flags = new byte[500];
    }

}
