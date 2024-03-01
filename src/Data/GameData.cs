using Godot;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


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
    private BattleCharacter clausCharacter;

    // The rest of the party members
    public List<CharacterEnum> party;
    public List<CharacterEnum> bench;

    //returns the corresponding character based on enum
    public BattleCharacter GetCharacter(CharacterEnum characterEnum)
    {
        switch (characterEnum)
        {
            case CharacterEnum.CLAUS:
                return clausCharacter;
            default:
                return null;
        }
    }
    //<summary>
    // Takes in character and sets the corresponding character to it. Uses .who to decide who is
    //</summary>
    public void UpdateCharacter(BattleCharacter newValue)
    {
        if (newValue.who == CharacterEnum.CLAUS)
            clausCharacter = newValue;
        else
            throw new System.ArgumentException("GameData.UpdateCharacter does not have " + newValue.who + " enum implemented.");
    }

    // All these functions handle the file writing itself. The UI should ask for confirmation/warn overwrite before calling these.
    public void Save(string filePath)
    {
        BinaryFormatter bf = new BinaryFormatter();
        Directory.CreateDirectory(Godot.ProjectSettings.GlobalizePath("user://save"));
        FileStream file = File.Open(Godot.ProjectSettings.GlobalizePath("user://save/" + filePath), FileMode.OpenOrCreate);

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
        clausCharacter = new BattleCharacter(CharacterEnum.CLAUS);
        party = new List<CharacterEnum>{CharacterEnum.CLAUS, CharacterEnum.NULL, CharacterEnum.NULL};
        bench = new List<CharacterEnum>();
        // flags = new byte[500];
    }

}
