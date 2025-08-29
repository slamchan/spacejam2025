using UnityEngine;

public class Building : MonoBehaviour
{

    [System.Serializable]
    public class UpgradeEffect
    {
        public string resourceType; // check TODO POSSIBLE EFFECTS
        public int amount;          // e.g. +10
    }


    [System.Serializable]
    public class UpgradeLevel
    {
        public GameObject prefab;     // Optional: new prefab for visual change
        public int woodCost;
        public int stoneCost;
        public int requiredTechLevel;
        public UpgradeEffect[] effects;  // instead of powerGain
    }

    public int ownerPlayerId = 1;
    public int currentLevel = 0;
    public UpgradeLevel[] upgradePath;

    private bool playerOnTop = false;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
                       TryUpgrade();
 
        }


        if (playerOnTop && Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryUpgrade();
        }
    }

    private void TryUpgrade()
    {
        if (currentLevel >= upgradePath.Length) return;

        var next = upgradePath[currentLevel];
        var res = ResourceManager.Instance.players[ownerPlayerId];

        if (res.techLevel < next.requiredTechLevel)
        {
            Debug.Log($"Player {ownerPlayerId}: Tech level too low for upgrade {currentLevel + 1}!");
            return;
        }

        if (ResourceManager.Instance.SpendResources(ownerPlayerId, next.woodCost, next.stoneCost))
        {
            // Apply effect
            ApplyEffects(next);

            // Swap prefab if defined
            if (next.prefab != null)
            {
                GameObject upgraded = Instantiate(next.prefab, transform.position, transform.rotation);
                var buildingComp = upgraded.GetComponent<Building>();
                buildingComp.ownerPlayerId = ownerPlayerId;
                buildingComp.currentLevel = currentLevel + 1;
                buildingComp.upgradePath = upgradePath;
                Destroy(gameObject);
            }
            else
            {
                currentLevel++;
            }

            Debug.Log($"Player {ownerPlayerId}: Upgraded building to level {currentLevel + 1}");
        }
        else
        {
            Debug.Log($"Player {ownerPlayerId} does not have enough resources!");
        }
    }

    private void ApplyEffects(UpgradeLevel level)
    {
        var owner = ResourceManager.Instance.players[ownerPlayerId];

        foreach (var effect in level.effects)
        {/*
        TODO POSSIBLE EFFECTS
            switch (effect.resourceType.ToLower())
            {
            
                case "power":
                    owner.resources.power += effect.amount;
                    break;
                case "food":
                    owner.resources.food += effect.amount;
                    break;
                case "gold":
                    owner.resources.gold += effect.amount;
                    break;
                default:
                    Debug.LogWarning($"Unknown effect type: {effect.resourceType}");
                    break;
            }*/
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player != null && player.playerId == ownerPlayerId)
            playerOnTop = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player != null && player.playerId == ownerPlayerId)
            playerOnTop = false;
    }
}
