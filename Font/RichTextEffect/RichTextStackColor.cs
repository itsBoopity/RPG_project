using Godot;

[Tool]
[GlobalClass]
public partial class RichTextStackColor : RichTextEffect
{
	public readonly string bbcode = "st";
	private readonly static Color color = new("#C390FF");

    public override bool _ProcessCustomFX(CharFXTransform charFX)
    {
		charFX.Color = color;
        return true;
    }
}