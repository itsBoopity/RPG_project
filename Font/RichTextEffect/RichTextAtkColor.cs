using Godot;

[Tool]
[GlobalClass]
public partial class RichTextAtkColor : RichTextEffect
{
	public readonly string bbcode = "atk";
	private readonly static Color color = new("#F38984");

    public override bool _ProcessCustomFX(CharFXTransform charFX)
    {
		charFX.Color = color;
        return true;
    }
}