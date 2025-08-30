using UnityEngine;
using System.Collections.Generic;

public class Research : Building
{
    public Building.UpgradeLevel[,] upgradePaths;
    public TechTree techTree;
    void Start()
    {
        // Assuming ownerPlayerId is set somewhere in your class
        owner = ResourceManager.Instance.players[ownerPlayerId];
        owner.hasResearchCenter = true;
        techTree = new TechTree();
    }



    protected override void NewUpdate()
    {
        base.NewUpdate();
        
        
        if (playerOnTop)
        {
            foreach (TechNode tech in techTree.roots)
            {
                Debug.Log(tech.name + tech.level);
            }
        }
    }
}
