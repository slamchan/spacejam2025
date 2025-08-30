using UnityEngine;
using TMPro;
using System.Globalization;

public class Building : MonoBehaviour
{

    [System.Serializable]
    public class UpgradeLevel
    {
        public int resCost;
        public string resType;
        public int requiredBuildLevel;
        public Sprite sprite;
    }

    public int ownerPlayerId = 1;
    public int currentLevel = 0;
    protected int buildingHp = 2;
    protected int buildingMaxHp = 2;
    public UpgradeLevel[] upgradePath;

    protected SpriteRenderer spriteRenderer; // Assign in Inspector

    protected int playerOnTopPlayerId = 0;

    protected bool playerOnTop = false;

    public string buildingName;


    public TMP_Text upgradeCostText; // assign in Inspector


    protected void Update()
    {
        NewUpdate();
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


    // Make these virtual so child classes can extend them
    protected virtual void NewAwake() { }
    protected virtual void NewUpdate() { }
    protected void ShowUpgradeCost()
    {
        if (currentLevel >= upgradePath.Length)
        {
            // Example: assuming next.cost is a PlayerResources-like object
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            upgradeCostText.text = $"{buildingName}\nMAX";
            upgradeCostText.gameObject.SetActive(true);
        }
        else
        {
            var next = upgradePath[currentLevel];
            if (next != null)
            {
                // Example: assuming next.cost is a PlayerResources-like object
                TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
                upgradeCostText.text = $"{buildingName}\n{textInfo.ToTitleCase(next.resType)}:{next.resCost}";
                upgradeCostText.gameObject.SetActive(true);
            }

        }


    }

    protected void HideUpgradeCost()
    {
        if (upgradeCostText != null)
            upgradeCostText.gameObject.SetActive(false);
    }


    protected void Awake()
    {
        NewAwake();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }


    protected void TryUpgrade()
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
            Upgrade(1);
        }
        else
        {
            Debug.Log($"Player {ownerPlayerId} does not have enough resources!");
        }
    }

    protected virtual void ApplyUpgradeEffects()
    {
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                playerOnTop = true;
                playerOnTopPlayerId = player.playerId;
            }
        }
        else
        {
            // Handle meteors
            Meteor meteor = collision.GetComponent<Meteor>();
            if (meteor != null)
            {
                buildingHp -= 1;
                meteor.TakeDamage(meteor.hp); // or just DestroyMeteor
                if (buildingHp <= 0 && currentLevel > 0)
                {
                    Upgrade(-1);

                }
                Debug.Log($"Level of the building is {currentLevel}");

            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {

        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            playerOnTop = false;
            playerOnTopPlayerId = 0;
        }
    }

    protected void Upgrade(int upg)
    {
        Debug.Log($"Level of the building is {currentLevel}");

        var next = upgradePath[currentLevel];

        if (next.sprite != null)
        {
            spriteRenderer.sprite = next.sprite; // next.sprite is the upgraded sprite
        }

        int HpIncrease = 2;
        buildingMaxHp = HpIncrease * (currentLevel + 1);
        buildingHp = buildingMaxHp;
        ApplyUpgradeEffects();
        currentLevel = currentLevel + upg;

        Debug.Log($"Player {ownerPlayerId}: Upgraded building to level {currentLevel}");
    }
}
