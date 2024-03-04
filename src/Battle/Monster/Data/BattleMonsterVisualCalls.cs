using Godot;

public partial class BattleMonster
{
	public Vector2 GetBoundary()
    {
        return visuals.GetBoundary();
    }

	public void ShowEstimate(int estimate)
    {
        visuals.ShowEstimate(estimate);
    }

    public void HideEstimate()
    {
        visuals.HideEstimate();
    }
}
