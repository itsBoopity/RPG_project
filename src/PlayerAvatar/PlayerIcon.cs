using Godot;
using System;

public class PlayerIcon : Node2D
{
    private Sprite hair;
    private Sprite hairBorder;
    private Sprite sideburn;
    private Sprite sideburnBorder;
    private Sprite moustache;
    private Sprite moustacheBorder;
    private Sprite beard;
    private Sprite beardBorder;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        hair = GetNode<Sprite>("Hair");
        hairBorder = GetNode<Sprite>("HairBorder");
        sideburn = GetNode<Sprite>("Sideburn");
        sideburnBorder = GetNode<Sprite>("SideburnBorder");
        moustache = GetNode<Sprite>("Moustache");
        moustacheBorder = GetNode<Sprite>("MoustacheBorder");
        beard = GetNode<Sprite>("Beard");
        beardBorder = GetNode<Sprite>("BeardBorder");

        UpdateIcon();
    }
    
    public void UpdateIcon()
    {
        AvatarData data = GetNode<MainEngine>("/root/MainEngine").gameData.avaData;

        if (data.hair == PlayerHair.NONE)
        {
            hair.Texture = null;
            hairBorder.Texture = null;
        }
        else
        {
            hair.Texture = GD.Load<Texture>("res://Images/PlayerIcon/Hair/" + data.hair + ".png");
            hairBorder.Texture = GD.Load<Texture>("res://Images/PlayerIcon/Hair/BORDER/" +  data.hair + ".png");
        }

        if (data.sideburn == PlayerSideburn.NONE)
        {
            sideburn.Texture = null;
            sideburnBorder.Texture = null;
        }
        else
        {
            sideburn.Texture = GD.Load<Texture>("res://Images/PlayerIcon/Sideburn/" + data.sideburn + ".png");
            sideburnBorder.Texture = GD.Load<Texture>("res://Images/PlayerIcon/Sideburn/BORDER/" + data.sideburn + ".png");
        }

        if (data.moustache == PlayerMoustache.NONE)
        {
            moustache.Texture = null;
            moustacheBorder.Texture = null;
        }
        else
        {
            moustache.Texture = GD.Load<Texture>("res://Images/PlayerIcon/Moustache/" + data.moustache + ".png");
            moustacheBorder.Texture = GD.Load<Texture>("res://Images/PlayerIcon/Moustache/BORDER/" +  data.moustache + ".png");
        }

        if (data.beard == PlayerBeard.NONE)
        {
            beard.Texture = null;
            beardBorder.Texture = null;
        }
        else
        {
            beard.Texture = GD.Load<Texture>("res://Images/PlayerIcon/Beard/" + data.beard + ".png");
            beardBorder.Texture = GD.Load<Texture>("res://Images/PlayerIcon/Beard/BORDER/" +  data.beard + ".png");
        }

        Utility.SetShaderColor(hair, data.hairColor);
        Utility.SetShaderColor(sideburn, data.hairColor);
        Utility.SetShaderColor(moustache, data.moustacheColor);
        Utility.SetShaderColor(beard, data.beardColor);   
    }

    private void FreeTextures()
    {
        if (hair.Texture != null) hair.Texture.Dispose();
        if (hairBorder.Texture != null) hairBorder.Texture.Dispose();
        if (sideburn.Texture != null) sideburn.Texture.Dispose();
        if (sideburnBorder.Texture != null) sideburnBorder.Texture.Dispose();
        if (beard.Texture != null) beard.Texture.Dispose();
        if (beardBorder.Texture != null) beardBorder.Texture.Dispose();
        if (moustache.Texture != null) moustache.Texture.Dispose();
        if (moustacheBorder.Texture != null) moustacheBorder.Texture.Dispose();
    }
}
