using Godot;
using System;

public class DialogueEngine : Node2D
{
    private File file;
    private Godot.Collections.Dictionary<string, DialogueActor> actors;

    private Node models;
    private InvisibleActor invisibleActor;
    private RichTextLabel dialogueBox;
    private RichTextLabel nameTag;
    private Node2D playerPortrait;
    private Sprite portrait;
    private Sprite shadow;
    private SceneTreeTween tween;
    private float dialogueSpeed;

    public override void _Ready()
    {
        actors = new Godot.Collections.Dictionary<string, DialogueActor>();
        file = new File();

        models = GetNode<Node>("Models");
        invisibleActor = GetNode<InvisibleActor>("Models/Invisible");
        dialogueBox = GetNode<RichTextLabel>("Dialogue/Text");
        nameTag = GetNode<RichTextLabel>("Dialogue/NameText");
        playerPortrait = GetNode<Node2D>("Portrait/PlayerPortrait");
        portrait = GetNode<Sprite>("Portrait");
        shadow = GetNode<Sprite>("Portrait/Shadow");
        tween = GetTree().CreateTween();
        tween.Stop();

        playerPortrait.Hide();
        SetProcessInput(false);
    }

    public void LoadDialogue(string path)
    {
        dialogueSpeed = Global.settings.dialogueSpeed;
        file.Open(path, File.ModeFlags.Read);
        SetProcessInput(true);
        AdvanceDialogue();
    }

    private void Finish()
    {
        //Play a fancy animation swooshing away the dialogue box.

        SetProcessInput(false);
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

        CharacterModel speaker;

        if (toParse[0] == "Player")
        {
            nameTag.BbcodeText = Global.data.avaData.name;
            playerPortrait.Show();
            portrait.Texture = null;
            shadow.Texture = portrait.Texture;

            speaker = invisibleActor.SetVoice(null);
        }
        else
        {
            DialogueActor actor = actors[toParse[0]];

            nameTag.BbcodeText = actor.tagName;
            playerPortrait.Hide();
            portrait.Texture = actor.LoadPortrait();
            shadow.Texture = portrait.Texture;

            // If model exists, play talk animation. Otherwise play bleeps independently using voicebleeper

            speaker = GetNodeOrNull<CharacterModel>("Models/" + toParse[0]);
            if (speaker == null)
                speaker = invisibleActor.SetVoice(actors[toParse[0]].LoadBleep());
        }
    
        string processed = 
            toParse[1].Replace("%Player%", Global.data.avaData.name);
            //.Replace("%he%", "")

        dialogueBox.VisibleCharacters = 0;
        dialogueBox.BbcodeText = processed;
        string[] separateGaps = dialogueBox.Text.Split('`');
        dialogueBox.BbcodeText = processed.Replace("`", "");
        
        int characterLength = separateGaps[0].Length;

        tween.Kill(); tween = GetTree().CreateTween();
        tween.TweenCallback(speaker, "Talk");
        tween.TweenProperty(dialogueBox, "visible_characters", characterLength, separateGaps[0].Length * dialogueSpeed);
        for (int i=1; i<separateGaps.Length; i++)
        {
            characterLength += separateGaps[i].Length;
            
            tween.TweenCallback(speaker, "StopTalk");
            tween.TweenInterval(dialogueSpeed * 90);
            tween.TweenCallback(speaker, "Talk");

            tween.TweenProperty(dialogueBox, "visible_characters", characterLength, separateGaps[i].Length * dialogueSpeed);
        }
        tween.TweenCallback(speaker, "StopTalk");
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
        foreach (Node i in models.GetChildren())
        {
            ((CharacterModel)i).StopTalk();
        }
    }
}
