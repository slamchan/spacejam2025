using UnityEngine;
using System.Collections.Generic;

    public class TechTree
    {
        private Dictionary<string, TechNode> nodes = new Dictionary<string, TechNode>();
        public TechNode Root { get; private set; }

        public TechNode AddNode(string id, string name, int cost, string parentId = null)
        {
            var node = new TechNode(id, name, cost);
            nodes[id] = node;

            if (parentId == null)
                Root = node;
            else
                node.SetParent(nodes[parentId]);

            return node;
        }

        public TechNode GetNode(string id) =>
            nodes.ContainsKey(id) ? nodes[id] : null;

        public bool ResearchTech(ResourceManager.PlayerResources player, string techId)
        {
            var node = GetNode(techId);
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


