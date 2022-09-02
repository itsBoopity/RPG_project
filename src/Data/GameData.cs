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

    public Character GetCharacter(CharacterEnum characterEnum)
    {
        switch (characterEnum)
        {
            case CharacterEnum.PLAYER:
                return playerCharacter;
            case CharacterEnum.CLAUS:
                return clausCharacter;
            default:
                return null;
        }
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
        playerCharacter = new Character(CharacterEnum.PLAYER);
        clausCharacter = new Character(CharacterEnum.CLAUS);
        party = new CharacterEnum[3] {CharacterEnum.PLAYER, CharacterEnum.CLAUS, CharacterEnum.NULL};
        bench = new List<CharacterEnum>();
        // flags = new byte[500];
    }

}
