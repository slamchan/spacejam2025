using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D), typeof(LineRenderer))]
public class Base : Building
{
    [Header("Shield Settings")]
    public int shieldHP = 0;
    public int maxShieldHP = 0;
    public float shieldRadius = 5f;

    private int startingShield = 50;

    [Header("Visualizer")]
    public LineRenderer shieldRenderer;
    public int segments = 50;

    private CircleCollider2D shieldCollider;

    private float timeSinceDamage = 0f;
    public float regenDelay = 30f;

    public GameObject slotPrefab;   // Empty "slot" marker prefab
    public float slotSpacing = 0f;  // Distance from base
    private List<BuildingSlot> slots = new List<BuildingSlot>();

    public MeteorSpawner meteorSpawner;

    protected override void NewAwake()
    {
        base.NewAwake();
        shieldCollider = GetComponent<CircleCollider2D>();
        shieldCollider.isTrigger = true;
        shieldCollider.radius = shieldRadius;

        if (shieldRenderer == null)
            shieldRenderer = GetComponent<LineRenderer>();

        shieldRenderer.positionCount = segments + 1;
        shieldRenderer.loop = true;
        shieldRenderer.useWorldSpace = false;
        shieldRenderer.widthMultiplier = 0.05f;
        DrawShield();
        UpdateShieldStatus();

    }
    protected override void NewUpdate()
    {
        base.NewUpdate();
        if (shieldHP <= 0)
        {
            timeSinceDamage += Time.deltaTime;
            if (timeSinceDamage >= regenDelay)
            {
                RegenerateShield();
            }
        }

        // Update collider and visualizer
        shieldCollider.radius = shieldRadius;
        DrawShield();
    }


    private void AddSlots(int countPerSide)
    {
        // Calculate base position
        Vector3 basePos = transform.position;

        for (int i = 0; i < countPerSide; i++)
        {
            float offset = (i + 1) * slotSpacing;

            // Left side slot
            Vector3 leftPos = basePos + Vector3.left * offset;
            CreateSlot(leftPos);

            // Right side slot
            Vector3 rightPos = basePos + Vector3.right * offset;
            CreateSlot(rightPos);
        }
    }

    private void CreateSlot(Vector3 pos)
    {
        GameObject slotObj = Instantiate(slotPrefab, pos, Quaternion.identity, transform);
        BuildingSlot slot = slotObj.GetComponent<BuildingSlot>();
        slots.Add(slot);
    }

    public List<BuildingSlot> GetSlots()
    {
        return slots;
    }

    private void RegenerateShield()
    {
        shieldHP += maxShieldHP;

        if (shieldHP > 0)
        {
            UpdateShieldStatus(); // re-enable shield
            timeSinceDamage = 0f; // reset timer
        }
    }

    protected void DrawShield()
    {
        float angleStep = 360f / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * shieldRadius;
            shieldRenderer.SetPosition(i, pos);
        }
    }

    protected void UpdateShieldStatus()
    {
        bool isActive = shieldHP > 0;
        if (shieldRenderer != null) shieldRenderer.enabled = isActive;
        if (shieldCollider != null) shieldCollider.enabled = isActive;
    }

    public void TakeDamage(int dmg)
    {
        shieldHP -= dmg;
        if (shieldHP <= 0)
        {
            shieldHP = 0;
            UpdateShieldStatus();
            Debug.Log("Base shield destroyed!");
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // Only respond if the trigger belongs to the BoxCollider2D for player detection
        if (collision.IsTouching(GetComponent<BoxCollider2D>()))
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
                }
            }
        }
        else
        {
            // Handle meteors
            Meteor meteor = collision.GetComponent<Meteor>();
            if (meteor != null)
            {

                TakeDamage(meteor.power);
                meteor.TakeDamage(meteor.hp); // or just DestroyMeteor
            }
        }
    }



    protected override void AssignWorker(int upg)
    {
        return;
    }


    protected override void ApplyUpgradeEffects(int upg)
    {
        base.ApplyUpgradeEffects(upg);
        maxShieldHP = currentLevel * startingShield;
        shieldRadius = 5f + currentLevel * 2f;
        if (maxShieldHP > shieldHP)
        {
            shieldHP += startingShield;
        }
        else
        {
            shieldHP = maxShieldHP;
        }
        UpdateShieldStatus();
        slotSpacing += 3;
        AddSlots(1);
        meteorSpawner.xRange += 3 * upg;
        meteorSpawner.ySpawn += 3 * upg;

        owner.maxPop += 4 * upg;
        owner.pop += 4 * upg;
        owner.availablePop += 4 * upg;
        owner.powerExpense += 2 * upg;
        owner.foodIncome -= 6 * upg;
        if (owner.pop > owner.maxPop)
        {
            owner.pop = owner.maxPop;
        }
    }

}
