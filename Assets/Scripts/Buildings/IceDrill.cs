using UnityEngine;

public class IceDrill : Building
{
    protected override void AssignWorker(int upg)
    {
        if (currentWorkers < currentLevel)
        {
            bool success = owner.AssingWorker(upg);
            if (success)
            {
                currentWorkers++;
                owner.waterIncome += 1 * upg * owner.waterMod;
                Debug.Log($"Assigned {upg} worker(s) to this building.");
            }
        }
    }
    protected override void ApplyUpgradeEffects(int upg)
    {
        owner.powerExpense += 2 * upg;
    }
}
