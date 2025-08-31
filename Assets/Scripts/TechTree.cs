using UnityEngine;
using System.Collections.Generic;

public class TechTree
{
    private Dictionary<string, TechNode> nodes = new Dictionary<string, TechNode>();
    public List<TechNode> roots = new List<TechNode>();

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

        TechNode node = new TechNode(idPrefix, name, newCost, newBaseCost, level);

        if (parentNode != null)
        {
            node.SetParent(parentNode);
        }
        else
        {
            roots.Add(node);
        }

        nodes[idPrefix + level] = node;
        return node;
    }


    public TechTree()
    {
        AddNode("oreDrillMaxLevel", "Max ore drill", 1);
        AddNode("iceDrillMaxLevel", "Max ice drill", 1);
        AddNode("farmDomeMaxLevel", "Max farming dome", 1);
        AddNode("habitatMaxLevel", "Max habitat", 1);
        AddNode("powerMaxLevel", "Max power plant", 1);
        AddNode("laserTurretMaxLevel", "Max laser turret", 2);
        AddNode("shieldMaxLevel", "Max shield", 3);
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

    public bool ResearchTech(ResourceManager.PlayerResources player, string idPrefix, int level)
    {
        var node = GetNode(idPrefix, level);
        if (node == null) return false;

        if (node.CanResearch(player))
        {
            player.techPoint -= node.cost;
            player.UnlockTech(node);
            return true;
        }

        return false;
    }
}


