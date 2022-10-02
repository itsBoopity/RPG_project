using Godot;
using System;

public class CharacterModelRack : Node2D
{
    private Godot.Collections.Dictionary<string, Node> models;

    public override void _ExitTree()
    {
        if (models == null) return;
        foreach(Node model in models.Values)
            model.QueueFree();
        models = null;
    }

    public void Initiate()
    {
        models = new Godot.Collections.Dictionary<string, Node>();
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

    public void FadeCharacter()
    {
        if (GetChildCount() != 0) 
            RemoveChild(GetChild(0));
    }
}
