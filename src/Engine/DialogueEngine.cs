using Godot;
using System;

public class DialogueEngine : Node2D
{
    private File file;
    private Godot.Collections.Dictionary<string, DialogueActor> actors;

    private Node models;
    private RichTextLabel dialogueBox;
    private RichTextLabel nameTag;
    private Node2D playerPortrait;
    private Sprite portrait;
    private Sprite shadow;
    private VoiceBleeper bleep;
    private SceneTreeTween tween;

    // 0.02f slow, 0.01f normal, 0.005f fast
    private float dialogueSpeed = 0.005f;

    public override void _Ready()
    {
        actors = new Godot.Collections.Dictionary<string, DialogueActor>();
        file = new File();

        models = GetNode<Node>("Models");
        dialogueBox = GetNode<RichTextLabel>("Dialogue/Text");
        nameTag = GetNode<RichTextLabel>("Name/Text");
        playerPortrait = GetNode<Node2D>("Portrait/PlayerPortrait");
        portrait = GetNode<Sprite>("Portrait");
        shadow = GetNode<Sprite>("Portrait/Shadow");
        bleep = GetNode<VoiceBleeper>("Bleep");
        tween = GetTree().CreateTween();
        tween.Stop();

        playerPortrait.Visible = false;
    }

    public void LoadDialogue(string path)
    {
        file.Open(path, File.ModeFlags.Read);
        AdvanceDialogue();
    }

    private void Finish()
    {
        //Play a fancy animation swooshing away the dialogue box. At the end of the animation, call a method that actually frees this.
        this.QueueFree();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
            AdvanceDialogue();
    }

    private void AdvanceDialogue()
    {
        if (!file.IsOpen()) return;

        StopBleeps();

        // Finish Text scroll
        if (tween.IsRunning())
        {
            dialogueBox.VisibleCharacters = -1;
            tween.Kill();
            return;
        }

        string[] toParse;
        do {
            toParse = file.GetLine().Split(new char[] {' '}, 2);
            if (toParse[0] == "")
                continue;
            else if (toParse[0] == "define")
                DefineActor(ref toParse[1]);
            else if (toParse[0] == "line")
                Line(ref toParse[1]);
            else if (toParse[0] == "show")
                ShowActor(ref toParse[1]);
            else
                GD.Print("Reading: " + toParse[0]);

        } while (toParse[0] != "line" && toParse[0] != "pause" && !file.EofReached());

        if (toParse[0] != "line" && file.EofReached())
            Finish();
    }

    private void DefineActor(ref string text)
    {
        string[] toParse = text.Split(' ');

        if (actors.ContainsKey(toParse[0]))
        {
            GD.Print("Dialogue trying to define Actor with a variable that already exists.");
            return;
        }

        if  (toParse.Length == 1)
            actors.Add(toParse[0], new DialogueActor(toParse[0]));
        else if (toParse.Length == 2)
            actors.Add(toParse[0], new DialogueActor(toParse[0], toParse[1]));
        else if (toParse.Length == 3)
            actors.Add(toParse[0], new DialogueActor(toParse[0], toParse[1], toParse[2]));
        else
            actors.Add(toParse[0], new DialogueActor(toParse[0], toParse[1], toParse[2], toParse[3]));
    }

    private void Line(ref string text)
    {
        string[] toParse = text.Split(new char[] {' '}, 2);

        if (toParse[0] == "Player")
        {
            nameTag.BbcodeText = GetNode<MainEngine>("/root/MainEngine").gameData.avaData.name;
            playerPortrait.Visible = true;
            portrait.Texture = null;
            shadow.Texture = portrait.Texture;
        }
        else
        {
            DialogueActor actor = actors[toParse[0]];

            nameTag.BbcodeText = actor.tagName;
            playerPortrait.Visible = false;
            portrait.Texture = actor.LoadPortrait();
            shadow.Texture = portrait.Texture;

            // If model exists, play talk animation. Otherwise play bleeps independently using voicebleeper
            CharacterModel speaker = GetNodeOrNull<CharacterModel>("Models/" + toParse[0]);
            if (speaker != null)
                speaker.Talk(toParse[1].Length);
            else
                bleep.Talk(actor.LoadBleep(),toParse[1].Length);

        }
    
        PrintDialogue(ref toParse[1]);
    }

    private void PrintDialogue(ref string line)
    {
        string processed = 
            line.Replace("%Player%", nameTag.BbcodeText = GetNode<MainEngine>("/root/MainEngine").gameData.avaData.name);
            //.Replace("%he%", "")
        
        string[] separateGaps = processed.Split('`');
        
        dialogueBox.BbcodeText = separateGaps[0];
        dialogueBox.VisibleCharacters = 0;
        int characterLength = separateGaps[0].Length;

        tween.Kill(); tween = GetTree().CreateTween();
        tween.TweenProperty(dialogueBox, "visible_characters", characterLength, separateGaps[0].Length * dialogueSpeed);
        for (int i=1; i<separateGaps.Length; i++)
        {
            characterLength += separateGaps[i].Length;
            dialogueBox.BbcodeText += separateGaps[i];
            tween.TweenInterval(dialogueSpeed * 90);
            tween.TweenProperty(dialogueBox, "visible_characters", characterLength, separateGaps[i].Length * dialogueSpeed);
        }
    }

    private void ShowActor(ref string text)
    {
        string[] toParse = text.Split(' ');

        if (!actors.ContainsKey(toParse[0]))
        {
            GD.Print("Dialogue trying to show actor that wasn't defined.");
            return;
        }

        if (GetNodeOrNull<CharacterModel>("Models/" + toParse[0]) == null)
        {
            CharacterModel loaded = actors[toParse[0]].LoadModel();
            if (loaded == null) {GD.Print("Dialogue trying to show actor that doesn't have model."); return;}
            models.AddChild(loaded);
            if (toParse.Length == 2)
                loaded.Position = new Vector2(toParse[1].ToInt(), loaded.Position.y);
        }
    }

    private void Expression(ref string text)
    {
        string[] toParse = text.Split(' ');

        if (!actors.ContainsKey(toParse[0]))
        {
            GD.Print("Dialogue trying to change expression of actor that wasn't defined.");
            return;
        }

        actors[toParse[0]].ChangeExpression(toParse[1]);

        CharacterModel model = GetNodeOrNull<CharacterModel>(toParse[0]);
        if (model != null) model.ChangeExpression(toParse[1]);
    }

    private void StopBleeps()
    {
        bleep.StopTalk();
        foreach (Node i in models.GetChildren())
        {
            ((CharacterModel)i).StopTalk();
        }
    }
}
