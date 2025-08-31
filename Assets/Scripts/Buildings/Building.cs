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
        public TechNode requiredTech;
        public Sprite sprite;
    }

    public int ownerPlayerId = 1;
    public int currentLevel = 0;
    protected int buildingHp = 2;
    protected int buildingMaxHp = 2;
    protected int currentWorkers = 0;
    public UpgradeLevel[] upgradePath;

    protected SpriteRenderer spriteRenderer; // Assign in Inspector

    protected int playerOnTopPlayerId = 0;

    protected bool playerOnTop = false;

    public string buildingName;
    protected ResourceManager.PlayerResources owner;


    public TMP_Text upgradeCostText;
    public int baseCost;


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
        // Handle Player 1 key input
        if (owner == ResourceManager.Instance.players[1]) // Player 1
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Example: Assign 1 worker
                AssignWorker(1);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Example: Assign 1 worker
                AssignWorker(-1);
            }
        }
        // Handle Player 2 key input
        else if (owner == ResourceManager.Instance.players[2]) // Player 2
        {
            if (Input.GetKeyDown(KeyCode.Minus))  // "-" key for Player 2
            {
                // Example: Assign 1 worker
                AssignWorker(1);
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                // Example: Assign 1 worker
                AssignWorker(-1);
            }
        }
    }


    void Start()
    {
        // Assuming ownerPlayerId is set somewhere in your class
        owner = ResourceManager.Instance.players[ownerPlayerId];
    }

    // Make these virtual so child classes can extend them
    protected virtual void NewAwake() { }
    protected virtual void NewUpdate() { }
    protected void ShowUpgradeCost()
    {
        if (currentLevel >= upgradePath.Length - 1)
        {
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            upgradeCostText.text = $"{buildingName}\nMAX\n{currentWorkers}/{currentLevel}";
            upgradeCostText.gameObject.SetActive(true);

            PositionUpgradeText();
        }

        else
        {
            var next = upgradePath[currentLevel];
            if (next != null)
            {
                TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
                upgradeCostText.text = $"{buildingName}\n{textInfo.ToTitleCase(next.resType)}:{next.resCost}\n{currentWorkers}/{currentLevel} ";
                upgradeCostText.gameObject.SetActive(true);

                PositionUpgradeText();
            }
        }
    }

    private void PositionUpgradeText()
    {
        if (upgradeCostText == null) return;

        RectTransform rt = upgradeCostText.GetComponent<RectTransform>();

        if (ownerPlayerId == 2)
        {
            // Right-center for Player 2
            rt.anchorMin = new Vector2(1, 0.5f);
            rt.anchorMax = new Vector2(1, 0.5f);
            rt.pivot = new Vector2(1, 0.5f);
            rt.anchoredPosition = new Vector2(-50, 0); // adjust offset
        }
        else
        {
            // Left-center for Player 1
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(0, 0.5f);
            rt.pivot = new Vector2(0, 0.5f);
            rt.anchoredPosition = new Vector2(50, 0); // adjust offset
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
        if (currentLevel >= upgradePath.Length - 1) return;

        var next = upgradePath[currentLevel];

        if (owner.maxBuildLevel < next.requiredBuildLevel)
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

    protected virtual void ApplyUpgradeEffects(int upg)
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
                player.canJump = false;
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
            player.canJump = true;
        }
    }

    protected virtual void AssignWorker(int upg)
    {
        if (owner != null)
        {
            if ((currentWorkers < currentLevel || upg < 0) && currentWorkers + upg >= 0)
            {
                bool success = owner.AssingWorker(upg);
                if (success)
                {
                    currentWorkers += upg;
                    Debug.Log($"Assigned {upg} worker(s) to this building.");
                }
            }
        }
    }

    protected void Upgrade(int upg)
    {
        Debug.Log($"Level of the building is {currentLevel}");
        currentLevel = currentLevel + upg;

        var next = upgradePath[currentLevel];

        if (next.sprite != null)
        {
            spriteRenderer.sprite = next.sprite; // next.sprite is the upgraded sprite
        }

        int HpIncrease = 1;
        buildingMaxHp = HpIncrease * (currentLevel + 1);
        buildingHp = buildingMaxHp;

        ApplyUpgradeEffects(upg);

        Debug.Log($"Player {ownerPlayerId}: Upgraded building to level {currentLevel}");
    }
}
