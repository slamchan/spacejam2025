using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

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

        public bool CanResearch(PlayerResources player)
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

        public bool ResearchTech(PlayerResources player, string techId)
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


    [System.Serializable]
    public class PlayerResources
    {
        public int ore = 10;
        public int water = 30;
        public int food = 30;
        public int pop = 3;
        public int power = 3;
        public int techPoint = 15;
        public int maxBuildLevel = 3;

        public float oreIncome = 1f;
        public float waterIncome = 1f;
        public float foodIncome = 1f;
        public float popIncome = 0.1f;
        public float powerIncome = 0.5f;
        public float techPointIncome = 0.2f;
        public HashSet<string> unlockedTechs = new HashSet<string>();

        public int this[string fieldName]
        {
            get
            {
                var field = typeof(PlayerResources).GetField(fieldName);
                return (int)field.GetValue(this);
            }
            set
            {
                var field = typeof(PlayerResources).GetField(fieldName);
                field.SetValue(this, value);
            }
        }

        public bool HasTech(string techId)
        {
            return unlockedTechs.Contains(techId);
        }

        public void UnlockTech(string techId)
        {
            unlockedTechs.Add(techId);
        }
        public float GetIncome(string fieldName)
        {
            return (float)typeof(PlayerResources).GetField(fieldName + "Income").GetValue(this);
        }
    }
    private string[] resourceTypes = { "ore", "water", "food", "pop", "power", "techPoint" };
    public float updateInterval = 5f; // update every 1 second
    private float timer = 0f;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer -= updateInterval; // reset timer
            UpdateResources();
        }
    }

    private void UpdateResources()
    {
        for (int i = 1; i <= 2; i++)
        {
            foreach (var res in resourceTypes)
            {
                int amount = Mathf.FloorToInt(players[i].GetIncome(res) * updateInterval);
                players[i][res] += amount;
            }
        }
    }


    public Dictionary<int, PlayerResources> players = new Dictionary<int, PlayerResources>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // init 2 players
        players[1] = new PlayerResources();
        players[2] = new PlayerResources();

        // Build a simple tree
        TechTree techTree = new TechTree();
        techTree.AddNode("buildingLevel1", "Upgrade max building level 1", 1);
        techTree.AddNode("buildingLevel2", "Upgrade max building level 2", 2, "buildingLevel1");
        techTree.AddNode("buildingLevel3", "Upgrade max building level 3", 3, "buildingLevel2");

        // Try researching
        bool success1 = techTree.ResearchTech(players[1], "buildingLevel1");
        Debug.Log($"Researched \"{techTree.GetNode("buildingLevel1").name}\": {success1}");
    }

    public bool SpendResources(int playerId, string resType, int cost)
    {
        var res = players[playerId];
        if (res[resType] >= cost)
        {
            res[resType] -= cost;
            return true;
        }
        return false;
    }

    public void AddResources(int playerId, string resType, int amount)
    {
        var res = players[playerId];
        res[resType] += amount;
    }

    public bool HasEnough(int playerId, string type, int requiredAmount)
    {
        var res = players[playerId];
        if (res[type] >= requiredAmount)
        {
            return true;
        }
        return false;
    }
}
