using System;

//Adding a new character: update, update constructor here, update character enums, and gameData GetCharacter(), newSave and serializtion

[Serializable]
public class Character: BattleFigure
{
    public CharacterEnum who;
    public Character() {}
    public Character(CharacterEnum character)
    {
        switch(character)
        {
            case CharacterEnum.PLAYER:
                who = CharacterEnum.PLAYER;
                HP = 10; maxHP = 10;
                ATK = 2;
                DEF = 2;
                break;
            case CharacterEnum.CLAUS:
                who = CharacterEnum.CLAUS;
                HP = 11; maxHP = 11;
                ATK = 3;
                DEF = 1;
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
}