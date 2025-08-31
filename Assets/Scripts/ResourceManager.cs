using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;


public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public GameObject resourcePrefabP1; // prefab with Image + TMP_Text
    public GameObject resourcePrefabP2; // prefab with Image + TMP_Text

    [System.Serializable]
    public class PlayerResources
    {
        public int ore = 10;
        public int water = 30;
        public int food = 30;
        public int pop = 3;
        public int availablePop = 3;
        public int maxPop = 5;
        public int power = 3;
        public int techPoint = 15;
        public int powerExpense = 0;
        public int maxBuildLevel = 3;
        public bool hasResearchCenter = false;



        public int oreMod = 4;
        public int foodMod = 6;
        public int powerMod = 3;
        public int waterMod = 4;


        public int oreIncome = 0;
        public int waterIncome = 0;
        public int foodIncome = 0;
        public float popIncome = 0.1f;
        public int powerIncome = 0;
        public float techPointIncome = 0.2f;
        public HashSet<TechNode> unlockedTechs = new HashSet<TechNode>();
        public TechTree techTree;

        public int GetMaxTechLevelByIdPrefix(string idPrefix)
        {
            return unlockedTechs
                .Where(node => node.idPrefix == idPrefix)
                .Select(node => node.level)
                .DefaultIfEmpty(0)
                .Max();
        }

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

        public bool HasTech(TechNode techId)
        {
            return unlockedTechs.Contains(techId);
        }

        public int calculateExpenses(string fieldname)
        {

            switch (fieldname)
            {
                case ("food"):
                    return pop;
                case ("power"):
                    return powerExpense;
                case ("water"):
                    return pop;

                default:
                    return 0;
            }
        }

        public void UnlockTech(TechNode tech)
        {
            unlockedTechs.Add(tech);
        }

        public bool AssingWorker(int number)
        {

            if (availablePop > 0 && number > 0)
            {
                availablePop -= number;
                return true;
            }
            else if (number < 0)
            {
                availablePop -= number;
                return true;
            }
            return false;
        }
        public int GetIncome(string fieldName)
        {
            return (int)typeof(PlayerResources).GetField(fieldName + "Income").GetValue(this);
        }
    }
    private string[] resourceTypes = { "ore", "water", "food", "power" };
    public float updateInterval = 5f; // update every 1 second
    private float timer = 0f;

    [Header("Player 1 UI")]
    public TMP_Text p1StatsText;

    [Header("Player 2 UI")]
    public TMP_Text p2StatsText;

    private void Update()
    {
        UpdateUI(resourcePrefabP1, resourcePrefabP2);
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

                int amount = players[i].GetIncome(res);
                int expenses = players[i].calculateExpenses(res);
                players[i][res] += amount - expenses;
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
        players[1].techTree = new TechTree();
        players[2].techTree = new TechTree();
        UpdateUI(resourcePrefabP1, resourcePrefabP2);
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


    private void UpdateUI(GameObject p1, GameObject p2)
    {


        string[] resourceTypes = { "techPoint", "water", "food", "power", "ore", "availablePop" };
        for (int i = 1; i <= 2; i++)
        {
            if (i == 1)
            {

                TMP_Text[] texts = p1.GetComponentsInChildren<TMP_Text>();

                foreach (var text in texts)
                {
                    // Optional: match by some identifier, e.g. text.name
                    string key = text.name; // assumes text.name matches resourceTypes
                    text.text = players[i][key].ToString();
                }

            }
            else
            {
                
                TMP_Text[] texts = p2.GetComponentsInChildren<TMP_Text>();

                foreach (var text in texts)
                {
                    // Optional: match by some identifier, e.g. text.name
                    string key = text.name; // assumes text.name matches resourceTypes

                    text.text = players[i][key].ToString();
                }
            }

        }
    }
}
