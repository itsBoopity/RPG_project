using Godot;

// Contains methods where data is passed to BattleUI for display.
public partial class BattleEngine : Control
{
    public void CharacterBarChanged()
    {
        Ui.UpdateCharacterBars(party);
        Ui.SwapCharacterModel(GetCurrentPartyMember());
    }
}
