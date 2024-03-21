using System;
using System.Collections.Generic;
using System.Linq;
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


    private List<SkillElement> elementGrabBag = new();
    

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
    /// Generates semi-random elements and spawns GEMCOUNT gems. Randomization algorithm described in detail in 
    /// </summary>
    private void RandomizeGems()
    {
        for (int i=0; i<GEMCOUNT; i++)
        {
            gemElements[i] = PullFromGemGrabBag();

        }

        for (int i=0; i<GEMCOUNT; i++)
        {
            Visuals.SpawnGem(i, gemElements[i]);
        }

        gemsLeft = GEMCOUNT;
    }

    /// <summary>
    /// Grab bag method randomization (every element at least once),
    /// with the added randomization of one element which appears twice in the grab bag every time it is refilled.
    /// </summary>
    /// <returns>Element pulled from the bag.</returns>
    private SkillElement PullFromGemGrabBag()
    {
        if (elementGrabBag.Count == 0)
        {
            RefillGrabBag();
        }

        SkillElement output = elementGrabBag[^1];
        elementGrabBag.RemoveAt(elementGrabBag.Count - 1);
        return output; 
    }

    private void RefillGrabBag()
    {
        elementGrabBag.Add(SkillElement.BLUNT);
        elementGrabBag.Add(SkillElement.SLASH);
        elementGrabBag.Add(SkillElement.PIERCE);
        elementGrabBag.Add(SkillElement.FIRE);
        elementGrabBag.Add(SkillElement.ICE);
        elementGrabBag.Add(SkillElement.LIGHTNING);
        elementGrabBag.Add(elementGrabBag[(int)(GD.Randi() % elementGrabBag.Count)]);
        int count = elementGrabBag.Count;
        for (int i=0; i<elementGrabBag.Count - 1; i++)
        {
            int swapIndex = i + (int)(GD.Randi() % (count - i));
            (elementGrabBag[swapIndex], elementGrabBag[i]) = (elementGrabBag[i], elementGrabBag[swapIndex]);
        }
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