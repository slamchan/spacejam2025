using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TechTree
{
    private Dictionary<string, TechNode> nodes = new Dictionary<string, TechNode>();
    public ArrayList roots = new ArrayList();

    public TechNode AddNode(string idPrefix, string name, int baseCost = 0, string parentId = null)
    {
        TechNode parentNode = null;
        int level = 1;
        int newCost = baseCost + level;
        int newBaseCost = baseCost;

        if (parentId != null && nodes.TryGetValue(parentId, out parentNode))
        {
            level = parentNode.level + 1;
            newCost = parentNode.cost + parentNode.baseCost + level;
            newBaseCost = parentNode.baseCost;
        }

        string newId = idPrefix + level;
        TechNode node = new TechNode(newId, name, newCost, newBaseCost, level);

        if (parentNode != null)
        {
            node.SetParent(parentNode);
        }
        else
        {
            roots.Add(node);
        }

        nodes[newId] = node;
        return node;
    }


    public TechTree()
    {
        AddNode("oreDrillMaxLevel", "Upgrade max ore drill", 1);
        AddNode("iceDrillMaxLevel", "Upgrade max ice drill", 1);
        AddNode("farmDomeMaxLevel", "Upgrade max farming dome", 1);
        AddNode("habitatMaxLevel", "Upgrade max habitat", 1);
        AddNode("powerMaxLevel", "Upgrade max power", 1);
        AddNode("laserTurretMaxLevel", "Upgrade max laser turret", 2);
        AddNode("shieldMaxLevel", "Upgrade max shield", 3);
        AddNode("oreProd", "Upgrade ore productivity", 3);
        AddNode("iceProd", "Upgrade ice productivity", 3);
        AddNode("foodProd", "Upgrade food productivity", 3);
        AddNode("powerProd", "Upgrade power productivity", 3);
    }

    public TechNode GetNodeById(string id) =>
        nodes.ContainsKey(id) ? nodes[id] : null;

    public TechNode GetNode(string idPrefix, int level)
    {
        string id = idPrefix + level;
        TechNode existing = GetNodeById(id);
        if (existing != null)
        {
            return existing;
        }
        if (level > 1)
        {
            TechNode parent = GetNode(idPrefix, level - 1);
            if (parent != null)
            {
                return AddNode(idPrefix, parent.name, parent.baseCost, idPrefix + (level - 1));
            }
        }
        return null;
    }

    public bool ResearchTech(ResourceManager.PlayerResources player, string name, int level)
    {
        var node = GetNode(name, level);
        if (node == null) return false;

        if (node.CanResearch(player))
        {
            player.techPoint -= node.cost;
            player.UnlockTech(node.id);
            return true;
        }

        return false;
    }
}


