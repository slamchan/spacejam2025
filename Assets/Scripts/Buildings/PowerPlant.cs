using UnityEngine;

public class PowerPlant : Building
{
    protected override void AssignWorker(int upg)
    {

        if ((currentWorkers < currentLevel || upg < 0) && currentWorkers + upg >= 0)
        {
            bool success = owner.AssingWorker(upg);
            if (success)
            {
                currentWorkers += upg;
                owner.powerIncome += 1 * upg * owner.powerMod;
                Debug.Log($"Assigned {upg} worker(s) to this building.");
            }
        }

    }
}
