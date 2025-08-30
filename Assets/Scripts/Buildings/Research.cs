using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Globalization;

public class Research : Building
{
    public List<List<TechLevel>> upgradePaths;
    public TechTree techTree;
    public int selectedTechIndex = 0;
    public TMP_Text selectedTechText;

    public class TechLevel : UpgradeLevel
    {
        public TechNode tech;
        public TechLevel(TechNode tech)
        {
            resCost = tech.cost;
            resType = "techPoint";
            requiredBuildLevel = tech.level - 1;
            this.tech = tech;
        }
    }
    private void AddNextTechToPath(List<TechLevel> upgradePath)
    {
        TechLevel lastIndex = upgradePath[upgradePath.Count - 1];
        TechNode tech = lastIndex.tech;
        TechNode nextTech = techTree.GetNode(tech.idPrefix, tech.level + 1);
        TechLevel nextLevel = new TechLevel(nextTech);
        upgradePath.Add(nextLevel);
    }
    void Start()
    {
        // Assuming ownerPlayerId is set somewhere in your class
        owner = ResourceManager.Instance.players[ownerPlayerId];
        owner.hasResearchCenter = true;
        upgradePaths = new List<List<TechLevel>>();
        techTree = new TechTree();
        foreach (TechNode tech in techTree.roots)
        {
            List<TechLevel> upgradePath = new List<TechLevel>();
            TechLevel level = new TechLevel(tech);
            upgradePath.Add(level);
            AddNextTechToPath(upgradePath);
            upgradePaths.Add(upgradePath);
        }
    }

    private void ShowSelectedTech()
    {
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        List<TechLevel> selectedTechPaths = upgradePaths[selectedTechIndex];
        TechLevel selectedTech = selectedTechPaths[selectedTechPaths.Count - 1];

        selectedTechText.text = $"{selectedTech.tech.name} level {selectedTech.tech.level}\n{selectedTech.resType}: {selectedTech.resCost}";
        selectedTechText.gameObject.SetActive(true);
    }



    protected override void NewUpdate()
    {
        base.NewUpdate();

        if (playerOnTop)
        {
            ShowSelectedTech();
        }

        if (playerOnTopPlayerId == 1 && Input.GetKeyDown(KeyCode.W))
        {

        }
        else if (playerOnTopPlayerId == 2 && Input.GetKeyDown(KeyCode.UpArrow))
        {

        }
        if (playerOnTopPlayerId == 1 && Input.GetKeyDown(KeyCode.S))
        {

        }
        else if (playerOnTopPlayerId == 2 && Input.GetKeyDown(KeyCode.DownArrow))
        {

        }
    }
}
