using UnityEngine;

public class Drill : Building
{
        protected override void AssingWorker(int upg)
    {
        base.AssingWorker(upg);
        owner.waterIncome += 1*upg;
    }
    protected override void ApplyUpgradeEffects(int upg)
    {
        owner.powerExpense += 2 * upg;
    }
}
