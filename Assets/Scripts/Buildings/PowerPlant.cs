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

    void Start()
    {
        owner = ResourceManager.Instance.players[ownerPlayerId];
        int maxLevel = owner.GetMaxTechLevelByIdPrefix("powerMaxLevel");
        for (int i = 0; i < maxLevel; i++)
        {
            UpgradeLevel level = new UpgradeLevel();
            if (i - 1 > 0)
            {
                level.resCost += (i - 1) * baseCost;
            }
            level.resCost += i * baseCost;
        }
    }
}
