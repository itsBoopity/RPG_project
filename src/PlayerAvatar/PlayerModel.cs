using Godot;
using System;

public class PlayerModel : CharacterModelBackup
{
    [Export] bool isPortrait = false;
    AvatarData data;
    private Sprite body;
    private Sprite arm;
    private Sprite palm;
    private Sprite head;
    private AnimatedSprite eye;
    private Sprite iris;
    private Sprite sclera;
    private Sprite nose;
    private Sprite mouth;
    private Sprite brow;
    private Sprite bodyhair;
    private Sprite bodyhairFront;
    private Sprite hair;
    private Sprite sideburn;
    private Sprite hairBack;
    private Sprite moustache;
    private Sprite beard;
    private Sprite scruff;
    private Sprite clothing;

    // Disables model animations such as blinking and breathing.

    public void CustomFree()
    {
        FreeTextures();
        QueueFree();
    }

    public override void _Ready()
    {
        if (isPortrait)
            base.StopAnimations();
            
        data = Global.data.avaData;
        if (data == null) throw new Exception("PlayerModel instanced when Global.data.avaData is not initialized.");
        
        base._Ready();
        body = GetNode<Sprite>("Body");
        arm = GetNode<Sprite>("Body/Arm");
        palm = GetNode<Sprite>("Body/Arm/Palm");
        head = GetNode<Sprite>("Body/Head");
        eye = GetNode<AnimatedSprite>("Body/Head/Eye");
        iris = GetNode<Sprite>("Body/Head/Iris");
        sclera = GetNode<Sprite>("Body/Head/Sclera");
        nose = GetNode<Sprite>("Body/Head/Nose");
        mouth = GetNode<Sprite>("Body/Head/Mouth");
        bodyhair  = GetNode<Sprite>("Body/BodyHair");
        bodyhairFront  = GetNode<Sprite>("Body/Arm/BodyHair");
        hair = GetNode<Sprite>("Body/Hair");
        hairBack = GetNode<Sprite>("Body/HairBack");
        sideburn = GetNode<Sprite>("Body/SideBurn");
        brow  = GetNode<Sprite>("Body/Head/Brow");
        beard = GetNode<Sprite>("Body/Beard");
        moustache = GetNode<Sprite>("Body/Moustache");
        scruff = GetNode<Sprite>("Body/Head/Scruff");
        clothing = GetNode<Sprite>("Body/Clothing");

        UpdateModel();
    }

    //Loads the images into the children nodes
    public void UpdateModel()
    {
        // FreeTextures();

        body.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Body/" + data.body + ".png");
        arm.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/FrontArm/" + data.body + ".png");
        palm.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/FrontArm/" + data.body + "_PALM.png");
        head.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Head/" + data.head + ".png");
        eye.Frames.ClearAll();
        eye.Frames.AddFrame("default", GD.Load<Texture>("res://Images/PlayerAvatar/Eye/" + data.eye + ".png"), 0);
        eye.Frames.AddFrame("default", GD.Load<Texture>("res://Images/PlayerAvatar/Eye/BLINK.png" ), 1);
        iris.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Iris/BASIC.png");

        if (data.skin == PlayerSkin.DARK || data.skin == PlayerSkin.DARK_YELLOW ||
            data.skin == PlayerSkin.DARKEST || data.skin == PlayerSkin.DARKEST_YELLOW)
             sclera.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Sclera/DEFAULT_DARK.png");
        else
            sclera.Texture =  GD.Load<Texture>("res://Images/PlayerAvatar/Sclera/DEFAULT_LIGHT.png");
    
        nose.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Nose/" + data.nose + ".png");
        mouth.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Mouth/SMILE.png");

        if (data.bodyHair)
        {
            bodyhair.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/BodyHair/" + data.body + ".png");
            bodyhairFront.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/BodyHair/" + data.body + "_FRONT.png");
        }
        else
        {
            bodyhair.Texture = null;
            bodyhairFront.Texture = null;
        }

        if (data.hair == PlayerHair.NONE)
            hair.Texture = null;
        else
            hair.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Hair/" + data.hair + ".png");
        if (data.hair == PlayerHair.CURLY || data.hair == PlayerHair.LONG)
            hairBack.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Hair/" + "/" + data.hair + "_BACK.png");
        else
            hairBack.Texture = null;
        
        if (data.sideburn == PlayerSideburn.NONE)
            sideburn.Texture = null;
        else 
            sideburn.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Sideburn/" + data.sideburn + ".png");

        if (data.brow == PlayerBrow.NONE)
            brow.Texture = null;
        else
            brow.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Brow/" + data.brow  + ".png");

        if (data.beard == PlayerBeard.NONE)
            beard.Texture = null;
        else
            beard.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Beard/" + data.beard + ".png");
        
        if (data.moustache == PlayerMoustache.NONE)
            moustache.Texture = null;
        else
            moustache.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Moustache/" + data.moustache + ".png");

        if (data.scruff == PlayerScruff.MOUSTACHE)
            scruff.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Scruff/" + data.scruff + ".png");
        else if (data.scruff == PlayerScruff.NONE)
            scruff.Texture = null;
        else
            scruff.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Scruff/" + data.scruff + "_" + data.head + ".png");

        clothing.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Clothing/" + data.clothing + "/" + data.body + ".png");

        Utility.SetShaderColor(body, data.skin);
        // All skin shares the same material, so it should change it for the rest automatically
        // Utility.SetShaderColor(arm, data.skin);
        // Utility.SetShaderColor(head, data.skin);
        // Utility.SetShaderColor(eye, data.skin);
        // Utility.SetShaderColor(mouth, data.skin);
        // Utility.SetShaderColor(nose, data.skin);

        Utility.SetShaderColor(iris, data.eyeColor);
        Utility.SetShaderColor(bodyhair, data.bodyhairColor);
        Utility.SetShaderColor(bodyhairFront, data.bodyhairColor);
        Utility.SetShaderColor(hair, data.hairColor);
        Utility.SetShaderColor(hairBack, data.hairColor);
        Utility.SetShaderColor(sideburn, data.hairColor);
        Utility.SetShaderColor(brow, data.browColor);
        Utility.SetShaderColor(beard, data.beardColor);
        Utility.SetShaderColor(moustache, data.moustacheColor);
        Utility.SetShaderColor(scruff, data.beardColor); 
    }

    private void FreeTextures()
    {
        //Technically the null checks are only necessary for the ones that are nullable during UpdateModel(), but in case it's necessary in the future
        if (body.Texture != null) body.Texture.Dispose();
        if (arm.Texture != null) arm.Texture.Dispose();
        if (palm.Texture != null) palm.Texture.Dispose();
        if (head.Texture != null) head.Texture.Dispose();
        if (eye.Frames.GetFrame("default", 0) != null) eye.Frames.GetFrame("default", 0).Dispose();
        if (eye.Frames.GetFrame("default", 1) != null) eye.Frames.GetFrame("default", 1).Dispose();
        if (iris.Texture != null) iris.Texture.Dispose();
        if (sclera.Texture != null) sclera.Texture.Dispose();
        if (nose.Texture != null) nose.Texture.Dispose();
        if (mouth.Texture != null) mouth.Texture.Dispose();
        if (bodyhair.Texture != null) bodyhair.Texture.Dispose();
        if (bodyhairFront.Texture != null) bodyhairFront.Texture.Dispose();
        if (hair.Texture != null) hair.Texture.Dispose();
        if (hairBack.Texture != null) hairBack.Texture.Dispose();
        if (sideburn.Texture != null) sideburn.Texture.Dispose();
        if (brow.Texture != null) brow.Texture.Dispose();
        if (beard.Texture != null) beard.Texture.Dispose();
        if (moustache.Texture != null) moustache.Texture.Dispose();
        if (scruff.Texture != null) scruff.Texture.Dispose();
        if (clothing.Texture != null) clothing.Texture.Dispose();
    }
}
