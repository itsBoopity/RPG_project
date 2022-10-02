using Godot;
using System;

public abstract class DungeonCard
{
    public string name;
    protected string description;
    protected DungeonEngine dungeonEngine;

    public void Initiate(DungeonEngine dungeonEngine) { this.dungeonEngine = dungeonEngine; }

    public string GetDescription() { return description + '\n' + GetDescriptionCustom(); }
    public virtual string GetDescriptionCustom() { return ""; }
    public abstract void UseCard(DungeonEngine dungeonEngine);
}
