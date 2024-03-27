public class BattleStatusDefend : BattleStatusBase, IOnSustainDamageValue
{
    public bool invalid;
    public int OnSustainDamageValueOrder => 100;

    protected override int InitialDuration => 1;

    public void ActivateSustainDamageValue(BattleFieldData bF, ref int damageValue)
    {
        damageValue = (int)(damageValue * 0.66);
    }

    public override void ConnectTriggers(BattleStatusArray array)
    {
        throw new System.NotImplementedException();
    }


}