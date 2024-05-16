using Godot;

[Tool]
[GlobalClass]
public partial class RichTextIfColor : RichTextEffect
{
	public readonly string bbcode = "if";
	private readonly static Color color = new("#C647E2");

    public override bool _ProcessCustomFX(CharFXTransform charFX)
    {
		charFX.Color = color;
        return true;
    }
}