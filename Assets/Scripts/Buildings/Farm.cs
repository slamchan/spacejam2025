using UnityEngine;

public class Farm : Building
{
    protected override void ApplyUpgradeEffects()
    {
        var owner = ResourceManager.Instance.players[ownerPlayerId];
        owner.foodIncome += 0.2f * (currentLevel + 1);
    }
}
