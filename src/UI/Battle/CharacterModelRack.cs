using Godot;
using System;

public class CharacterModelRack : Node2D
{
    private Godot.Collections.Dictionary<string, Node> models = new Godot.Collections.Dictionary<string, Node>();

    public override void _ExitTree()
    {
        foreach(Node model in models.Values)
            model.Free();
    }
    
    public void ShowCharacter(CharacterEnum who)
    {
        string name = who.ToString();
        if (!models.ContainsKey(name))
        {
            models.Add(name, GD.Load<PackedScene>("res://Objects/CharacterModel/" + name + ".tscn").Instance());
        }
        if (GetChildCount() != 0)
            RemoveChild(GetChild(0));
        AddChild(models[name]);
    }
}
