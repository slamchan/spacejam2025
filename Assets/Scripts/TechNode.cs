using UnityEngine;
using System.Collections.Generic;

    public class TechNode
    {
        public string id;
        public string name;
        public int cost;
        public TechNode parent;
        public List<TechNode> children = new List<TechNode>();

        public TechNode(string id, string name, int cost)
        {
            this.id = id;
            this.name = name;
            this.cost = cost;
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

