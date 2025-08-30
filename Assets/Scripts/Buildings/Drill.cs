using UnityEngine;

public class Drill : Building
{
    protected override void ApplyUpgradeEffects()
    {
        var owner = ResourceManager.Instance.players[ownerPlayerId];
        owner.waterIncome += 0.2f * (currentLevel + 1);
    }
}
