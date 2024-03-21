using System;
using System.Collections.Generic;
using Godot;
public partial class RainbowSentry: MonsterStats
{
    /// <summary>
    /// Currently only implemented for GEMCOUNT = 3
    /// </summary>
    private const int GEMCOUNT = 3;
    

    /// <summary>
    /// The element that each gem currently has.
    /// </summary>
    private SkillElement[] gemElements = new SkillElement[GEMCOUNT];

    /// <summary>
    /// How many gems are left unbroken.
    /// </summary>
    private int gemsLeft;
    

    new private RainbowSentryVisuals Visuals { get {return (RainbowSentryVisuals)base.Visuals; } }

    public override void Initialize()
    {
        RandomizeGems();
    }

    public override void LoadUpcomingTurn(BattleFieldData bf)
    {
        EmitSignal(SignalName.SignalIntent, 0, 0);
    }

    public override void AppendageHit(MonsterAppendage appendage)
    {
        if (appendage.appendageId >= 0 && appendage.appendageId <= 2)
        {
            Visuals.BreakGem(appendage.appendageId);
            gemsLeft -= 1;
            SetWeakness(gemElements[appendage.appendageId]);
            if (gemsLeft == 0)
            {
                RespawnGems();
            }
            EmitSignal(SignalName.SignalHitResult, 1.0f);
        }
        else
        {
            EmitSignal(SignalName.SignalHitResult, 0.0f);
        }
    }

    /// <summary>
    /// Generates random elements and spawns GEMCOUNT gems. Always creates at least one PHYSICAL and MAGICAL.
    /// </summary>
    private void RandomizeGems()
    {
        bool containsPhysical = false;
        bool containsMagical = false;
        for (int i=0; i<GEMCOUNT; i++)
        {
            gemElements[i] = SkillElementUtility.GetRandomPhysicalOrMagical();
            if (gemElements[i].IsPhysical())
            {
                containsPhysical = true;
            }
            else
            {
                containsMagical = true;
            }
        }

        if (!containsPhysical)
        {
            gemElements[0] = SkillElementUtility.GetRandomPhysical();
        }
        else if (!containsMagical)
        {
            gemElements[0] = SkillElementUtility.GetRandomMagical();
        }

        for (int i=0; i<GEMCOUNT; i++)
        {
            Visuals.SpawnGem(i, gemElements[i]);
        }

        gemsLeft = GEMCOUNT;
    }

    /// <summary>
    /// Waits for 1 second, then randomizes gems.
    /// </summary>
    private async void RespawnGems()
    {
        await ToSignal(Utility.Instance.CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);
        RandomizeGems();
    }

    private void SetWeakness(SkillElement element)
    {
        foreach (String key in elementalAffinity.Keys)
        {
            elementalAffinity[key] = 0.25f;
        }
        elementalAffinity[element.ToString()] = 2.0f;
    }
}