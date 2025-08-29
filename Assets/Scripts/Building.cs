using UnityEngine;
using TMPro;
using System.Globalization;

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
        public int resCost;
        public string resType;
        public int requiredBuildLevel;
        public UpgradeEffect[] effects;  // instead of powerGain

        public Sprite sprite;
    }

    public int ownerPlayerId = 1;
    public int currentLevel = 0;
    public UpgradeLevel[] upgradePath;

    private SpriteRenderer spriteRenderer; // Assign in Inspector

    private int playerOnTopPlayerId = 0;

    private bool playerOnTop = false;


    public TMP_Text upgradeCostText; // assign in Inspector


    private void Update()
    {
        if (playerOnTop)
        {
            ShowUpgradeCost();
        }
        else
        {
            HideUpgradeCost();
            return;
        }

        if (playerOnTopPlayerId == 1 && Input.GetKeyDown(KeyCode.S))
        {

            TryUpgrade();
        }
        else if (playerOnTopPlayerId == 2 && Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryUpgrade();
        }
    }

    private void ShowUpgradeCost()
    {
        var next = upgradePath[currentLevel];
        if (next != null)
        {
            // Example: assuming next.cost is a PlayerResources-like object
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            upgradeCostText.text = $"{textInfo.ToTitleCase(next.resType)}:{next.resCost}";
            upgradeCostText.gameObject.SetActive(true);
        }
    }

    private void HideUpgradeCost()
{
    if (upgradeCostText != null)
        upgradeCostText.gameObject.SetActive(false);
}


    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void TryUpgrade()
    {
        if (currentLevel >= upgradePath.Length) return;

        var next = upgradePath[currentLevel];
        var res = ResourceManager.Instance.players[ownerPlayerId];

        if (res.maxBuildLevel < next.requiredBuildLevel)
        {
            Debug.Log($"Player {ownerPlayerId}: Tech level too low for upgrade {currentLevel + 1}!");
            return;
        }

        if (ResourceManager.Instance.SpendResources(ownerPlayerId, next.resType, next.resCost))
        {
            // Apply effect
            ApplyEffects(next);

            if (next.sprite != null)
            {
                spriteRenderer.sprite = next.sprite; // next.sprite is the upgraded sprite
            }
            currentLevel += 1;

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

        PlayerController player = collision.GetComponent<PlayerController>();
        Debug.Log($"{player}");

        if (player != null)
        {
            playerOnTop = true;
            playerOnTopPlayerId = player.playerId; // 1 or 2
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            playerOnTop = false;
            playerOnTopPlayerId = 0;
        }
    }

}
