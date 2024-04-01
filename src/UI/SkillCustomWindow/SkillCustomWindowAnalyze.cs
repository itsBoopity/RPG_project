using Godot;

public partial class SkillCustomWindowAnalyze : SkillCustomWindow
{
    private BattleActor user;
    public override void Open(BattleFieldData bF, BattleInteractionData bI)
    {
        user = bI.user;
        GetNode<AnimationPlayer>("ThrowAnimation").Play((GD.Randi() % 3).ToString());

        BattleMonster monster = (BattleMonster)bI.target;
        GetNode<TextureRect>("Illustration").Texture = GD.Load<Texture2D>($"res://Images/MonsterIllustration/{monster.Id}.png");
        GetNode<Label>("Name").Text = monster.DisplayName;
        GetNode<RichTextLabel>("Description").Text = monster.AnalysisData;
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");

        GetNode<Label>("%BluntValue").Text = (bI.target.GetAffinity(SkillElement.BLUNT) * 100).ToString("F0") + "%";
        GetNode<Label>("%SlashValue").Text = (bI.target.GetAffinity(SkillElement.SLASH) * 100).ToString("F0") + "%";
        GetNode<Label>("%PierceValue").Text = (bI.target.GetAffinity(SkillElement.PIERCE) * 100).ToString("F0") + "%";

        GetNode<Label>("%FireValue").Text = (bI.target.GetAffinity(SkillElement.FIRE) * 100).ToString("F0") + "%";
        GetNode<Label>("%IceValue").Text = (bI.target.GetAffinity(SkillElement.ICE) * 100).ToString("F0") + "%";
        GetNode<Label>("%LightningValue").Text = (bI.target.GetAffinity(SkillElement.LIGHTNING) * 100).ToString("F0") + "%";
    }

    protected override void CleanUp() {}

    protected override void CancelCleanUp()
    {
        EmitSignal(SignalName.ReturnData, new BattleInteractionData(user));
    }
}
