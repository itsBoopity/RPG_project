using Godot;
using System.Collections.Generic;

/// <summary>
/// Manages model nodes by storing them in a Dictionary, and instancing and showing only the one that is currently displayed.
/// </summary>
public partial class CharacterModelRack : Control
{
    private Dictionary<string, Node2D> models = new();

    public override void _ExitTree()
    {
        if (models == null) return;
        foreach(Node model in models.Values)
            model.QueueFree();
        models.Clear();
    }
    
    public void ShowCharacter(CharacterEnum who)
    {
        string name = who.ToString().Capitalize();
        if (!models.ContainsKey(name))
        {
            models.Add(name, GD.Load<PackedScene>("res://Objects/CharacterModel/" + name + "Model.tscn").Instantiate<Node2D>());
            models[name].Material = GD.Load<Material>("res://ShaderMaterial/CanvasGroupDropShadow.tres");
        }
        if (GetChildCount() != 0)
            RemoveChild(GetChild(0));
        AddChild(models[name]);
    }

    public void FadeCharacter()
    {
        if (GetChildCount() != 0) 
            RemoveChild(GetChild(0));
    }
}
