using Godot;

public static class CalculationFormula
{
	/// <summary>
    /// Calculates output damage using formula of [ attackCoefficient * userAttack - targetDefense) * elementalAffinity * appendageCoef ].
    /// Clamped to do at least 1 damage if result is lower.
    /// </summary>
    public static int BasicDamage(int userAttack, int targetDefense, float elementalAffinity, float appendageCoef = 1, float attackCoefficient = 1)
    {
        int damage = Mathf.RoundToInt((userAttack * attackCoefficient * elementalAffinity - targetDefense) * appendageCoef);
        return (damage < 1) ? 1 : damage;
    }
}