using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [System.Serializable]
    public class PlayerResources
    {
        public int ore = 10;
        public int water = 30;
        public int food = 30;
        public int pop = 3;
        public int power = 3;
        public int techPoint = 0;
        public int techLevel = 1;
    }

    public enum ResourceTypes
    {
        Ore = "ore",
        Water = "water",
        Food = "food",
        Pop = "pop",
        Power = "power",
        TechPoint = "techPoint",
        TechLevel = "techLevel"
    }

    public Dictionary<int, PlayerResources> players = new Dictionary<int, PlayerResources>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // init 2 players
        players[1] = new PlayerResources();
        players[2] = new PlayerResources();
    }

    public bool SpendRes(int playerId, ResourceTypes resType, int cost)
    {
        var res = players[playerId];
        if (res[resType] >= cost)
        {
            res[resType] -= cost;
            return true;
        }
        return false;
    }

    public void AddRes(int playerId, ResourceTypes resType, int amount)
    {
        var res = players[playerId];
        res[resType] += amount;
    }
}
