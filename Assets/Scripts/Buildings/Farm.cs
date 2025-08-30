using UnityEngine;

public class Farm : Building
{
    protected override void AssignWorker(int upg)
    {
        if (currentWorkers < currentLevel)
        {
            bool success = owner.AssingWorker(upg);
            if (success)
            {
                currentWorkers++;
                owner.foodIncome += 1 * upg * owner.foodMod;
                Debug.Log($"Assigned {upg} worker(s) to this building.");
            }
        }
    }
    protected override void ApplyUpgradeEffects(int upg)
    {
        owner.powerExpense += 1 * upg;
    }
}
