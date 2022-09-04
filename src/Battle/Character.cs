using System;

//Adding a new character: update, update constructor here, update character enums, and gameData GetCharacter(), newSave and serializtion

[Serializable]
public class Character: BattleFigure
{
    public CharacterEnum who;
    public bool turnActive = true;
    public Character() {}
    public Character(CharacterEnum character)
    {
        switch(character)
        {
            case CharacterEnum.Player:
                who = CharacterEnum.Player;
                HP = 10; maxHP = 10;
                ATK = 2;
                DEF = 2;
                skills.Add(new FirstStrike());
                break;
            case CharacterEnum.Claus:
                who = CharacterEnum.Claus;
                HP = 11; maxHP = 11;
                ATK = 3;
                DEF = 1;
                skills.Add(new FirstStrike());
                break;
        }
    }
    public Character Clone()
    {
        Character clone = new Character();
        base.Clone(clone);
        clone.who = who;

        return clone;
    }

    // Resets parameters after fight: sets stacks according to level, and resets skill cooldowns
    public Character Reset()
    {
        turnActive = true;
        stack = 0;
        foreach (BattleSkill skill in skills)
            skill.Reset();
        return this;
    }
}