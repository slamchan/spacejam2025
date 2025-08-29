using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [System.Serializable]
    public class PlayerResources
    {
        public int wood = 100;
        public int stone = 50;
        public int power = 0;
        public int techLevel = 1;
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

    public bool SpendResources(int playerId, int woodCost, int stoneCost)
    {
        var res = players[playerId];
        if (res.wood >= woodCost && res.stone >= stoneCost)
        {
            res.wood -= woodCost;
            res.stone -= stoneCost;
            return true;
        }
        return false;
    }

    public void AddPower(int playerId, int amount)
    {
        players[playerId].power += amount;
    }

    public int GetTechLevel(int playerId)
    {
        return players[playerId].techLevel;
    }

    public void IncreaseTechLevel(int playerId)
    {
        players[playerId].techLevel++;
    }
}
