using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingSlot : MonoBehaviour
{
    public List<GameObject> availableBuildings;  // Assign in inspector
    private int selectedIndex = 0;

    public bool isOccupied = false;
    public GameObject currentBuilding;

    private int playerOnTopId = -1;

    [Header("UI")]
    public TMP_Text upgradeCostText;  // Assign in inspector


    private void Update()
    {
        if (isOccupied || playerOnTopId == -1) return;

        // Player 1
        if (playerOnTopId == 1)
        {
            if (Input.GetKeyDown(KeyCode.W))
                ScrollSelection();
            if (Input.GetKeyDown(KeyCode.S))
                PlaceSelected(playerOnTopId);
        }
        // Player 2
        else if (playerOnTopId == 2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                ScrollSelection();
            if (Input.GetKeyDown(KeyCode.DownArrow))
                PlaceSelected(playerOnTopId);
        }
    }

    private void ScrollSelection()
    {
        selectedIndex = (selectedIndex + 1) % availableBuildings.Count;
        ShowSelectedBuildingInfo();
    }

    private void PlaceSelected(int ownerPlayerId)
    {
        if (isOccupied) return;

        GameObject buildingPrefab = availableBuildings[selectedIndex];
        currentBuilding = Instantiate(buildingPrefab, transform.position, Quaternion.identity);
        Building b = currentBuilding.GetComponent<Building>();
        if (b != null)
        {
            b.ownerPlayerId = ownerPlayerId;
        }
        isOccupied = true;

        // Hide UI
        if (upgradeCostText != null)
            upgradeCostText.text = "";

        // Hide the slot visually (optional: scale to zero or disable renderer)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;

        // Optionally disable the collider so the player can walk through
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }


    private void ShowSelectedBuildingInfo()
    {
        if (upgradeCostText == null) return;

        GameObject buildingPrefab = availableBuildings[selectedIndex];
        upgradeCostText.text = buildingPrefab.name.ToUpper();
        // You could expand later to also show costs
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            playerOnTopId = player.playerId;
            player.canJump = false; // or whatever your PlayerController uses
            ShowSelectedBuildingInfo();
                        PositionUpgradeText();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null && player.playerId == playerOnTopId)
        {
            playerOnTopId = -1;
            if (upgradeCostText != null)
                upgradeCostText.text = ""; // Clear text when player leaves

            player.canJump = true;
        }
    }

    
    private void PositionUpgradeText()
    {
        if (upgradeCostText == null) return;

        RectTransform rt = upgradeCostText.GetComponent<RectTransform>();

        if (playerOnTopId == 2)
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

}
