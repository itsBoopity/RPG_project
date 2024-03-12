using Godot;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text.Json;
using System;


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
    private CharacterStats csClaus;
    private CharacterStats csYellam;
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
        switch (characterEnum)
        {
            case CharacterEnum.CLAUS:
                return csClaus;
            case CharacterEnum.YELLAM:
                return csYellam;
            default:
                throw new ArgumentException($"GameData::GetCharacter does not recognize enum: {characterEnum}");
        }
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
        List<BattleCharacter> output = new List<BattleCharacter>();
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
        List<BattleCharacter> output = new List<BattleCharacter>();
        foreach (CharacterEnum who in bench)
        {
            output.Add(new BattleCharacter(GetCharacter(who)));
        }
        return output;
    }

    
    public void Save(string filePath)
    {
        Directory.CreateDirectory(ProjectSettings.GlobalizePath("user://save"));
        FileStream file = File.Open(ProjectSettings.GlobalizePath("user://save/" + filePath), FileMode.OpenOrCreate);

        // string oneElement = JsonSerializer.Serialize<CharacterData>(cdClaus)

        //BinaryFormatter is obsolete and dangerous, use something else instead
        // bf.Serialize(file, "v0.0.1");
        // bf.Serialize(file, avaData);
        // bf.Serialize(file,playerCharacter);
        // bf.Serialize(file,clausCharacter);
        // bf.Serialize(file,party);
         



        file.Close();
    }
    public void Load(string filePath) //filePath is passed to this function through the slot selection
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Godot.ProjectSettings.GlobalizePath("user://save/" + filePath), FileMode.Open);

        //BinaryFormatter is obsolete and dangerous, use something else instead
        // string saveVersion = (string) bf.Deserialize(file);
        // playerCharacter = (Character) bf.Deserialize(file);
        // clausCharacter = (Character) bf.Deserialize(file);
        // party = (CharacterEnum[]) bf.Deserialize(file);
        //flags = (byte[]) bf.Deserialize(file);

        file.Close();
    }

    public void newSave() // Create New Save
    {
        csClaus = GD.Load<CharacterStats>("res://Resources/Stats/Character/Claus.tres");
        csYellam = GD.Load<CharacterStats>("res://Resources/Stats/Character/Yellam.tres");
        party = new List<CharacterEnum>{CharacterEnum.CLAUS, CharacterEnum.YELLAM, CharacterEnum.NULL};
        bench = new List<CharacterEnum>();
        // flags = new byte[500];
    }

}
