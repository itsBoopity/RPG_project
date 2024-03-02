using System.Collections.Generic;

public partial class BattleCharacter: BattleActor
{
    public CharacterEnum who;
    private BattleCharacter() {}
    public BattleCharacter(string name, CharacterEnum who, int hp, int maxHp, int atk, int def, int spd, List<BattleSkill> skills)
    {
        this.name = name;
        this.who = who;
        this.hp = hp;
        this.maxHp = maxHp;
        this.atk = atk;
        this.def = def;
        this.spd = spd;
        this.skills = skills;
    }
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
                skills.Add(new SkillFirstStrike());
                skills.Add(new SkillPrecisionNeedle());
                break;
        }
        Reset();
    }
    public BattleCharacter Clone()
    {
        BattleCharacter clone = new();
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