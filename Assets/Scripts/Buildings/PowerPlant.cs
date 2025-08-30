using UnityEngine;

public class PowerPlant : Building
{
    protected override void ApplyUpgradeEffects(int upg)
    {
        owner.powerIncome -= 3 * upg;
    }
}
