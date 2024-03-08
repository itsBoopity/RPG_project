/// <summary>
/// Enum representing whether a skill is usable.
/// If not, indicates reason why.
/// </summary>
public enum SkillUsableResult
{
    USABLE,
	IN_COOLDOWN,
	NOT_ENOUGH_STACKS,
	CHARACTER_NOT_ACTIVE,
	SPECIAL_REQUIREMENT_NOT_MET
}