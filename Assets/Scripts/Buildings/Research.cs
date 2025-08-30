using UnityEngine;

public class Research : Building
{
    void Start()
    {
        // Assuming ownerPlayerId is set somewhere in your class
        owner = ResourceManager.Instance.players[ownerPlayerId];
        owner.hasResearchCenter = true;
    }
}
