using Godot;

//This class is very redundant, but there are a few animation functions that the portrait doesn't need
public class PlayerPortrait: Node2D
{
    AvatarData data;
    private Sprite body;
    private Sprite head;
    private AnimatedSprite eye;
    private Sprite iris;
    private Sprite sclera;
    private Sprite nose;
    private Sprite mouth;
    private Sprite brow;
    private Sprite bodyhair;
    private Sprite hair;
    private Sprite sideburn;
    private Sprite hairBack;
    private Sprite moustache;
    private Sprite beard;
    private Sprite scruff;
    private Sprite clothing;

    public void CustomFree()
    {
        FreeTextures();
        QueueFree();
    }

    public override void _Ready()
    {
        data = Global.data.avaData;
    
        body = GetNode<Sprite>("Body");
        head = GetNode<Sprite>("Body/Head");
        eye = GetNode<AnimatedSprite>("Body/Head/Eye");
        iris = GetNode<Sprite>("Body/Head/Iris");
        sclera = GetNode<Sprite>("Body/Head/Sclera");
        nose = GetNode<Sprite>("Body/Head/Nose");
        mouth = GetNode<Sprite>("Body/Head/Mouth");
        bodyhair  = GetNode<Sprite>("Body/BodyHair");
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

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_left"))
        {
            data.Randomize(); UpdateModel();
        }
    }

    //Loads the images into the children nodes
    public void UpdateModel()
    {
        FreeTextures();      

        body.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Body/" + data.skin + "/" + data.body + ".png");
        head.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Head/" + data.skin + "/" + data.head + ".png");
        eye.Frames.ClearAll();
        eye.Frames.AddFrame("default", GD.Load<Texture>("res://Images/PlayerAvatar/Eye/" + data.skin + "/" + data.eye  + ".png"), 0);
        eye.Frames.AddFrame("default", GD.Load<Texture>("res://Images/PlayerAvatar/Eye/" + data.skin + "/BLINK.png" ), 1);
        iris.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Iris/" + data.eyeColor + ".png");

        if (data.skin == PlayerSkin.DARK || data.skin == PlayerSkin.DARK_YELLOW ||
            data.skin == PlayerSkin.DARKEST || data.skin == PlayerSkin.DARKEST_YELLOW)
             sclera.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Sclera/DEFAULT_DARK.png");
        else
            sclera.Texture =  GD.Load<Texture>("res://Images/PlayerAvatar/Sclera/DEFAULT_LIGHT.png");
    
        nose.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Nose/" + data.skin + "/" + data.nose + ".png");
        mouth.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Mouth/" + data.skin + "/SMILE.png");

        if (data.bodyHair)
        {
            bodyhair.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/BodyHair/" + data.bodyhairColor + "/" + data.body + ".png");
        }
        else
        {
            bodyhair.Texture = null;
        }

        if (data.hair == PlayerHair.NONE)
            hair.Texture = null;
        else
            hair.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Hair/" + data.hairColor + "/" + data.hair + ".png");
        if (data.hair == PlayerHair.CURLY || data.hair == PlayerHair.LONG)
            hairBack.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Hair/" + data.hairColor + "/" + data.hair + "_BACK.png");
        else
            hairBack.Texture = null;
        
        if (data.sideburn == PlayerSideburn.NONE)
            sideburn.Texture = null;
        else 
            sideburn.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/SideBurn/" + data.hairColor + "/" + data.sideburn + ".png");

        if (data.brow == PlayerBrow.NONE)
            brow.Texture = null;
        else
            brow.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Brow/" + data.browColor + "/" + data.brow  + ".png");

        if (data.beard == PlayerBeard.NONE)
            beard.Texture = null;
        else
            beard.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Beard/" + data.beardColor + "/" + data.beard + ".png");
        
        if (data.moustache == PlayerMoustache.NONE)
            moustache.Texture = null;
        else
            moustache.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Moustache/" + data.moustacheColor + "/" + data.moustache + ".png");

        if (data.scruff == PlayerScruff.MOUSTACHE)
            scruff.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Scruff/" + data.beardColor + "/" + data.scruff + ".png");
        else if (data.scruff == PlayerScruff.NONE)
            scruff.Texture = null;
        else
            scruff.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Scruff/" + data.beardColor + "/" + data.scruff + "_" + data.head + ".png");

        clothing.Texture = GD.Load<Texture>("res://Images/PlayerAvatar/Clothing/" + data.clothing + "/" + data.body + ".png");

    }

    private void FreeTextures()
    {
        //Technically the null checks are only necessary for the ones that are nullable during UpdateModel(), but in case it's necessary in the future
        if (body.Texture != null) body.Texture.Dispose();
        if (head.Texture != null) head.Texture.Dispose();
        if (eye.Frames.GetFrame("default", 0) != null) eye.Frames.GetFrame("default", 0).Dispose();
        if (eye.Frames.GetFrame("default", 1) != null) eye.Frames.GetFrame("default", 1).Dispose();
        if (iris.Texture != null) iris.Texture.Dispose();
        if (sclera.Texture != null) sclera.Texture.Dispose();
        if (nose.Texture != null) nose.Texture.Dispose();
        if (mouth.Texture != null) mouth.Texture.Dispose();
        if (bodyhair.Texture != null) bodyhair.Texture.Dispose();
        if (hair.Texture != null) hair.Texture.Dispose();
        if (hairBack.Texture != null) hairBack.Texture.Dispose();
        if (sideburn.Texture != null) sideburn.Texture.Dispose();
        if (brow.Texture != null) brow.Texture.Dispose();
        if (beard.Texture != null) beard.Texture.Dispose();
        if (moustache.Texture != null) moustache.Texture.Dispose();
        if (scruff.Texture != null) scruff.Texture.Dispose();
        if (clothing.Texture != null) clothing.Texture.Dispose();
    }
    
    public void ChangeExpression(string expression)
    {
        
    }
}
