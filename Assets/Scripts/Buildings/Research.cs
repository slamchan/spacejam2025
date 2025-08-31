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
        techTree = owner.techTree;
        foreach (TechNode tech in techTree.roots)
        {
            List<TechLevel> upgradePath = new List<TechLevel>();
            TechLevel level = new TechLevel(tech);
            upgradePath.Add(level);
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

    private void HideSelectedTech()
    {
        if (selectedTechText != null)
        {
            selectedTechText.gameObject.SetActive(false);
        }
    }

    private void ScrollSelection()
    {
        selectedTechIndex = (selectedTechIndex + 1) % upgradePaths.Count;
        ShowSelectedTech();
    }

    private new void TryUpgrade()
    {
        List<TechLevel> selectedTechPaths = upgradePaths[selectedTechIndex];
        TechLevel selectedTech = selectedTechPaths[selectedTechPaths.Count - 1];
        ResourceManager.PlayerResources player = ResourceManager.Instance.players[playerOnTopPlayerId];
        if (techTree.ResearchTech(player, selectedTech.tech.idPrefix, selectedTech.tech.level))
        {
            AddNextTechToPath(selectedTechPaths);
        }
        Debug.Log(player.techPoint);
    }



    protected override void NewUpdate()
    {
        base.NewUpdate();

        if (playerOnTop)
        {
            ShowSelectedTech();
        }
        else
        {
            HideSelectedTech();
            return;
        }

        if (playerOnTopPlayerId == 1 && Input.GetKeyDown(KeyCode.W))
        {
            ScrollSelection();
        }
        else if (playerOnTopPlayerId == 2 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            ScrollSelection();
        }
        if (playerOnTopPlayerId == 1 && Input.GetKeyDown(KeyCode.S))
        {
            TryUpgrade();
        }
        else if (playerOnTopPlayerId == 2 && Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryUpgrade();
        }
    }
}
