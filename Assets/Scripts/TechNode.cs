using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class TechNode
{
    public string id;
    public string name;
    public int cost;
    public int baseCost;
    public int level;
    public TechNode parent;
    public List<TechNode> children = new List<TechNode>();

    public TechNode(string id, string name, int cost, int baseCost, int level)
    {
        this.id = id + level;
        this.name = name;
        this.cost = cost;
        this.baseCost = baseCost;
        this.level = level;
    }

    public void SetParent(TechNode parent)
    {
        this.parent = parent;
        this.parent.children.Add(this);
    }

    public bool CanResearch(ResourceManager.PlayerResources player)
    {
        // Parent must be unlocked (unless this is the root node)
        if (parent != null && !player.HasTech(parent.id))
            return false;

        // Must have enough tech points
        if (player.techPoint < cost)
            return false;

        return true;
    }
}

