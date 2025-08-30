using UnityEngine;

public class Farm : Building
{

    protected override void ApplyUpgradeEffects(int upg)
    {
        owner.powerExpense += 1 * upg;
    }
}
