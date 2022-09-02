using Godot;
using System;

public class CustomizationButton : TextureButton
{
    private Vector2 originalSize;
    private float expandFactor = 1.15f;
    private float midExpandFactor = 1.07f;
    private PlayerEnum type;
    private int index;
    private Selector selector;
    public override void _Ready()
    {
        selector = GetNode<Selector>("../../Selector");
        originalSize = RectScale;
    }
    public void Initialize(PlayerEnum type, int index)
    {
        this.type = type;
        this.index = index;
        GetNode<Sprite>("Icon").Texture = GD.Load<Texture>
            ("res://Images/PlayerIcon/Customization/" + type + "/" + Utility.PlayerEnumName(type, index) + ".png");
    }

    public void ScaleBack()
    {
        RectScale = originalSize;
    }

    public void ScaleMid()
    {
        RectScale = originalSize * midExpandFactor;
    }

    public void ScaleUp()
    {
        RectScale = originalSize * expandFactor;
    }

    public void OnPress()
    {
        selector.Select(this);
        GetNode<CharacterCreator>("../../../../../../../").ChangeOption(type, index);
    }

    public void FreeTexture()
    {
        GetNode<Sprite>("Icon").Texture.Dispose();   
    }
}
