using System;
using Godot;

//Holds the enums of the player avatar 
[Serializable]
public class AvatarData
{
    public AvatarData() { Default(); }

    public string name;

    //0 for he/him, 1 for they/them
    public int pronoun;
    public PlayerSkin skin;
    public PlayerBody body;
    public PlayerHead head;
    public PlayerEye eye;
    public PlayerNose nose;
    public bool bodyHair;
    public PlayerHair hair;
    public PlayerSideburn sideburn;
    public PlayerBrow brow;
    public PlayerMoustache moustache;
    public PlayerBeard beard;
    public PlayerScruff scruff;

    public PlayerEyeColor eyeColor;
    public PlayerColor bodyhairColor;
    public PlayerColor hairColor; //Sideburn inherits hairColor;
    public PlayerColor browColor;
    public PlayerColor moustacheColor;
    public PlayerColor beardColor; //Scruff inherits beardColor

    public PlayerClothing clothing;

    public void Default() //Loads defaults when starting character customization
    {
        name = "Deffal";
        pronoun = 0;
        clothing = PlayerClothing.BASIC_TSHIRT;
        Randomize();
    }

    public void Randomize()
    {
        skin = (PlayerSkin) (GD.Randi() % Enum.GetNames(typeof(PlayerSkin)).Length);
        body = (PlayerBody) (GD.Randi() % Enum.GetNames(typeof(PlayerBody)).Length);
        head = (PlayerHead) (GD.Randi() % Enum.GetNames(typeof(PlayerHead)).Length);
        eye = (PlayerEye) (GD.Randi() % Enum.GetNames(typeof(PlayerEye)).Length);
        nose = (PlayerNose) (GD.Randi() % Enum.GetNames(typeof(PlayerNose)).Length);
        bodyHair = false;
        hair = (PlayerHair) (GD.Randi() % Enum.GetNames(typeof(PlayerHair)).Length);
        sideburn = (PlayerSideburn) (GD.Randi() % Enum.GetNames(typeof(PlayerSideburn)).Length);
        brow = (PlayerBrow) (GD.Randi() % Enum.GetNames(typeof(PlayerBrow)).Length);
        beard = (PlayerBeard) (GD.Randi() % Enum.GetNames(typeof(PlayerBeard)).Length);
        scruff = (PlayerScruff) (GD.Randi() % Enum.GetNames(typeof(PlayerScruff)).Length);
        moustache = (PlayerMoustache) (GD.Randi() % Enum.GetNames(typeof(PlayerMoustache)).Length);

        hairColor = (PlayerColor) (GD.Randi() % Enum.GetNames(typeof(PlayerColor)).Length);
        browColor = hairColor;
        beardColor = hairColor;
        moustacheColor = hairColor;
        bodyhairColor = hairColor;
        eyeColor = (PlayerEyeColor) (GD.Randi() % Enum.GetNames(typeof(PlayerEyeColor)).Length);
    }

    public void RandomizeColors()
    {
        skin = (PlayerSkin) (GD.Randi() % Enum.GetNames(typeof(PlayerSkin)).Length);
        hairColor = (PlayerColor) (GD.Randi() % Enum.GetNames(typeof(PlayerColor)).Length);
        browColor = hairColor;
        beardColor = hairColor;
        moustacheColor = hairColor;
        bodyhairColor = hairColor;
        eyeColor = (PlayerEyeColor) (GD.Randi() % Enum.GetNames(typeof(PlayerEyeColor)).Length);
    }
}
