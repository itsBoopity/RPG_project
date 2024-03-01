public partial class BattleCharacter: BattleActor
{
    public CharacterEnum who;
    public BattleCharacter() {}
    public BattleCharacter(CharacterEnum character)
    {
        switch(character)
        {
            case CharacterEnum.CLAUS:
                name = "Claus";
                who = CharacterEnum.CLAUS;
                hp = 11; maxHp = 11;
                atk = 3;
                def = 1;
                spd = 8;
                skills.Add(new FirstStrike());
                skills.Add(new PrecisionNeedle());
                break;
        }
        Reset();
    }
    public BattleCharacter Clone()
    {
        BattleCharacter clone = new BattleCharacter();
        base.Clone(clone);
        clone.who = who;

        return clone;
    }

    // Resets parameters after fight: sets stacks according to level, and resets skill cooldowns
    public BattleCharacter Reset()
    {
        turnActive = true;
        stack = level;
        foreach (BattleSkill skill in skills)
            skill.Reset();
        return this;
    }
}