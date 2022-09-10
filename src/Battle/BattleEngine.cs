using Godot;
using System.Collections.Generic;

// Untested: Finish()
// Barely tested: Initiate()
public class BattleEngine : Node2D
{
    private int selectedCharacter = -1;
    public List<Character> party;
    public List<Character> bench;
    public List<Monster> monsters;
    public BattleTimer timer;

    private Node partyNode;
    private Node monsterNode;
    private Node skillNode;
    private CharacterModelRack characterModels;
    private RichTextLabel infoLabel;

    public override void _Ready()
    {
        timer = GetNode<BattleTimer>("%Timer");
        partyNode = GetNode("%PartyBar");
        characterModels = GetNode<CharacterModelRack>("CharacterModels");
        monsterNode = GetNode("%Monsters");
        skillNode = GetNode("%Skills");
        infoLabel = GetNode<RichTextLabel>("%InfoLabel");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("battle_character1"))
            SelectCharacter(0);
        else if (@event.IsActionPressed("battle_character2"))
            SelectCharacter(1);
        else if (@event.IsActionPressed("battle_character3"))
            SelectCharacter(2);
    }
    private void PlayerTurn()
    {
        infoLabel.BbcodeText = "Player Turn";
        foreach (Monster monster in monsters)
            monster.LoadUpcomingTurn(this);
        
        foreach (Character character in party)
            character.turnActive = true;

        // Tick down cooldowns, statusi etc.

        if (Global.settings.timerEnabled)
            timer.StartTimer(20);
    }

    private void EnemyTurn()
    {
        infoLabel.BbcodeText = "Enemy Turn";
        foreach (Monster monster in monsters)
            monster.ExecuteTurn(this);

        UpdateUI(); // This should instead be animated and updated by each skill
        PlayerTurn();
    }

    private void ExecuteSkill(int index)
    {
        if (selectedCharacter == -1)
            return;
        BattleSkill skill = party[selectedCharacter].skills[index];
        if (skill.type == SkillType.OFFENSIVE)
            ExecuteOffensive(party[selectedCharacter].skills[index]);
    }

    private void ExecuteOffensive(BattleSkill skill)
    {
        infoLabel.BbcodeText = "Enemy Targetting";
    }

    //Loads all the data and calls UpdateUI() at the end
    public void Initiate(BattleSetup battleSetup)
    {
        GameData data = Global.data;
        selectedCharacter = -1;

        party = new List<Character>();

        foreach (CharacterEnum i in data.party)
        {
            if (data.GetCharacter(i) != null)
            {
                Character toAdd = data.GetCharacter(i).Clone();
                party.Add(toAdd);
            }
        }

        foreach (CharacterEnum i in data.bench)
        {
            Character toAdd = data.GetCharacter(i).Clone();
                bench.Add(toAdd);
        }
        
        monsters = new List<Monster>();
        foreach (int i in battleSetup.monsterID)
            monsters.Add(MonsterFactory.Create(i));
        foreach (Monster monster in monsters)
        {
            monsterNode.AddChild(monster.GetModel());
        }

        SelectCharacter(0);
        PlayerTurn();
    }

    //Called upon finishing the fight. Performs cleanup and finalizing, such as saving character data and giving rewards
    private void Finish()
    {
        GameData data = Global.data;
        foreach (Character i in party)
        {
            Character character = data.GetCharacter(i.who);
            character = i.Reset();
        }
        foreach (Character i in bench)
        {
            Character character = data.GetCharacter(i.who);
            character = i.Reset();
        }
    }

    private void SelectCharacter(int index)
    {
        // Generate spots for healing/buff/utility

        if (index == selectedCharacter || index >= party.Count) return;
        selectedCharacter = index;

        foreach (CharacterBar bar in partyNode.GetChildren())
            bar.Unselect();
        partyNode.GetChild<CharacterBar>(index).Select();

        UpdateUI();
    }

    //Updates all UI with current data
    private void UpdateUI()
    {
        for (int i=0; i<3; i++)
        {
            if (i < party.Count)
                partyNode.GetChild<CharacterBar>(i).Update(party[i]);
            else
                partyNode.GetChild<CharacterBar>(i).Hide();
        }

        if (selectedCharacter != -1)
        {
            characterModels.ShowCharacter(party[selectedCharacter].who);
            
            for (int i=0; i<5; i++)
            {
                if (i < party[selectedCharacter].skills.Count)
                    skillNode.GetChild<SkillBoxUI>(i).Update(party[selectedCharacter].skills[i]);
                else
                    skillNode.GetChild<SkillBoxUI>(i).Update(null);
            }
        }
        else
        {
            //Hide the appropriate UIs
        }
    }
}
