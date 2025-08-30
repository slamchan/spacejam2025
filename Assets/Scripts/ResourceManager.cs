using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public MeteorSpawner spawnerP1;
    public MeteorSpawner spawnerP2;



    [System.Serializable]
    public class PlayerResources
    {
        public int ore = 10;
        public int water = 30;
        public int food = 30;
        public int pop = 3;
        public int power = 3;
        public int techPoint = 17;
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

    [Header("Player 1 UI")]
    public TMP_Text p1OreText;
    public TMP_Text p1WaterText;
    public TMP_Text p1FoodText;
    public TMP_Text p1PopText;
    public TMP_Text p1PowerText;
    public TMP_Text p1TechText;

    [Header("Player 2 UI")]
    public TMP_Text p2OreText;
    public TMP_Text p2WaterText;
    public TMP_Text p2FoodText;
    public TMP_Text p2PopText;
    public TMP_Text p2PowerText;
    public TMP_Text p2TechText;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer -= updateInterval; // reset timer
            UpdateResources();
            UpdateUI();
        }
    }

    private void UpdateResources()
    {
        for (int i = 1; i <= 2; i++)
        {
            foreach (var res in resourceTypes)
            {
                int amount = Mathf.FloorToInt(players[i].GetIncome(res));
                players[i][res] += amount;
            }
            spawnerP1.TriggerSpawn();
            spawnerP2.TriggerSpawn();
           // spawner.difficultyLevel++;
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
        UpdateUI();
        // Build a simple tree
        TechTree techTree = new TechTree();
        TechNode techNode;
        // Try researching
        bool success1 = techTree.ResearchTech(players[1], "oreDrillMaxLevel", 1);
        techNode = techTree.GetNode("oreDrillMaxLevel", 1);
        Debug.Log($"Researched \"{techNode.name}\" Level 1 with cost {techNode.cost}: {success1}");
        Debug.Log($"Current tech points left: {players[1].techPoint}");

        bool fail1 = techTree.ResearchTech(players[1], "oreDrillMaxLevel", 3);
        techNode = techTree.GetNode("oreDrillMaxLevel", 3);
        Debug.Log($"Researched \"{techNode.name}\" Level 3 with cost {techNode.cost}: {fail1}");
        Debug.Log($"Current tech points left: {players[1].techPoint}");

        bool success2 = techTree.ResearchTech(players[1], "oreDrillMaxLevel", 2);
        techNode = techTree.GetNode("oreDrillMaxLevel", 2);
        Debug.Log($"Researched \"{techNode.name}\" Level 2 with cost {techNode.cost}: {success2}");
        Debug.Log($"Current tech points left: {players[1].techPoint}");

        bool success3 = techTree.ResearchTech(players[1], "oreDrillMaxLevel", 3);
        techNode = techTree.GetNode("oreDrillMaxLevel", 3);
        Debug.Log($"Researched \"{techNode.name}\" Level 3 with cost {techNode.cost}: {success3}");
        Debug.Log($"Current tech points left: {players[1].techPoint}");

        bool fail2 = techTree.ResearchTech(players[1], "oreDrillMaxLevel", 4);
        techNode = techTree.GetNode("oreDrillMaxLevel", 4);
        Debug.Log($"Researched \"{techNode.name}\" Level 4 with cost {techNode.cost}: {fail2}");
        Debug.Log($"Current tech points left: {players[1].techPoint}");
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
    private void UpdateUI()
    {
        // P1
        p1OreText.text = "Ore: " + players[1]["ore"];
        p1WaterText.text = "Water: " + players[1]["water"];
        p1FoodText.text = "Food: " + players[1]["food"];
        p1PopText.text = "Pop: " + players[1]["pop"];
        p1PowerText.text = "Power: " + players[1]["power"];
        p1TechText.text = "Tech: " + players[1]["techPoint"];

        // P2
        p2OreText.text = "Ore: " + players[2]["ore"];
        p2WaterText.text = "Water: " + players[2]["water"];
        p2FoodText.text = "Food: " + players[2]["food"];
        p2PopText.text = "Pop: " + players[2]["pop"];
        p2PowerText.text = "Power: " + players[2]["power"];
        p2TechText.text = "Tech: " + players[2]["techPoint"];
    }
}
