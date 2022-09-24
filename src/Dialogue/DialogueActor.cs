using Godot;

public class DialogueActor: Godot.Object
{
    public string tagName;
    public string pathBleep;
    public string pathPortrait;
    public string pathModel;
    public string expression;

    private string keyName;

    ~DialogueActor() { this.Free(); }
    public DialogueActor(string tagName, string pathBleep = "", string pathPortrait = "", string pathModel = "")
    {
        keyName = tagName;
        this.tagName = tagName;
        this.pathBleep = pathBleep;
        this.pathPortrait = pathPortrait;
        this.pathModel = pathModel;
        expression = "default";
    }
    public void ChangeExpression(string value) {expression = value;}

    public Texture LoadPortrait()
    {
        if (pathPortrait == "")
            return null;
        else
            return GD.Load<Texture>(pathPortrait + '/' + expression + ".png");
    }

    public CharacterModel LoadModel()
    {
        if (pathModel == "")
            return null;
        CharacterModel loaded = GD.Load<PackedScene>(pathModel).Instance<CharacterModel>();
        loaded.Name = keyName;
        return loaded;
    }

    public AudioStream LoadBleep()
    {
        if (pathBleep == "")
            return null;
        return GD.Load<AudioStream>(pathBleep);
    }
}
