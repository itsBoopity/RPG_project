using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


// Save File Structure:
// Version Number = helps with conversion of old files
// currentScene = to see if the player is in dungeon, in the middle of dialogue, or in the village (maybe not all this)
// data based on currentScene (dungeon state / dialogue file name and line being parsed)
// AvatarData
// CharacterMembers in order
// Party
// bench
// Flags
public class GameData
{
    public AvatarData avaData;
    public Character playerCharacter;
    public Character clausCharacter;
    // The rest of the party members 
    public CharacterEnum[] party;
    public List<CharacterEnum> bench;
    //public byte[] flags;

    //returns the corresponding character based on enum
    public Character GetCharacter(CharacterEnum characterEnum)
    {
        switch (characterEnum)
        {
            case CharacterEnum.Player:
                return playerCharacter;
            case CharacterEnum.Claus:
                return clausCharacter;
            default:
                return null;
        }
    }
    //<summary>
    // Takes in character and sets the corresponding character to it. Uses .who to decide who is
    //</summary>
    public void UpdateCharacter(Character newValue)
    {
        if (newValue.who == CharacterEnum.Player)
            playerCharacter = newValue;
        else if (newValue.who == CharacterEnum.Claus)
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

        bf.Serialize(file, "v0.0.1");
        bf.Serialize(file, avaData);
        bf.Serialize(file,playerCharacter);
        bf.Serialize(file,clausCharacter);
        bf.Serialize(file,party);

        file.Close();
    }
    public void Load(string filePath) //filePath is passed to this function through the slot selection
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Godot.ProjectSettings.GlobalizePath("user://save/" + filePath), FileMode.Open);

        string saveVersion = (string) bf.Deserialize(file);
        avaData = (AvatarData) bf.Deserialize(file);
        playerCharacter = (Character) bf.Deserialize(file);
        clausCharacter = (Character) bf.Deserialize(file);
        party = (CharacterEnum[]) bf.Deserialize(file);
        //flags = (byte[]) bf.Deserialize(file);

        file.Close();
    }

    public void newSave() // Create New Save
    {
        avaData = new AvatarData();
        playerCharacter = new Character(CharacterEnum.Player);
        clausCharacter = new Character(CharacterEnum.Claus);
        party = new CharacterEnum[3] {CharacterEnum.Player, CharacterEnum.Claus, CharacterEnum.Null};
        bench = new List<CharacterEnum>();
        // flags = new byte[500];
    }

}
