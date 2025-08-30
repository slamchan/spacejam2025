using UnityEngine;

public class PowerPlant : Building
{

    protected override void ApplyUpgradeEffects()
    {
        var owner = ResourceManager.Instance.players[ownerPlayerId];
        owner.powerIncome += 2.2f * (currentLevel + 1);
    }
}
