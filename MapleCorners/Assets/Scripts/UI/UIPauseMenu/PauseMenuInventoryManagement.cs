// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuInventoryManagement : MonoBehaviour
{
    [SerializeField] private PauseMenuInventoryManagementSlot[] inventoryManagementSlot = null;

    public GameObject inventoryManagementDraggedItemPrefab;

    [SerializeField] private Sprite transparent16x16 = null;

    [HideInInspector] public GameObject inventoryTextBoxGameobject;


    private void OnEnable()
    {
        // Subscribe to InventoryUpdatedEvent
        EventHandler.InventoryUpdatedEvent += PopulatePlayerInventory;

        // Populate the inventory of the player
        if (InventoryManager.Instance != null)
        {
            PopulatePlayerInventory(InventoryLocation.player, InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player]);
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from InventoryUpdatedEvent
        EventHandler.InventoryUpdatedEvent -= PopulatePlayerInventory;

        // Destroy the inventory text box
        DestroyInventoryTextBoxGameobject();
    }

    /// <summary>
    /// Destroys the inventory text box
    /// </summary>
    public void DestroyInventoryTextBoxGameobject()
    {
        if (inventoryTextBoxGameobject != null)
        {
            Destroy(inventoryTextBoxGameobject);
        }
    }

    /// <summary>
    /// Destroy any items currently being dragged
    /// </summary>
    public void DestroyCurrentlyDraggedItems()
    {
        for (int i = 0; i < InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player].Count; i++)
        {
            if (inventoryManagementSlot[i].draggedItem != null)
            {
                Destroy(inventoryManagementSlot[i].draggedItem);
            }

        }
    }

    /// <summary>
    /// Populates the inventory slots on the Inventory Management screen
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="playerInventoryList"></param>
    private void PopulatePlayerInventory(InventoryLocation inventoryLocation, List<InventoryItem> playerInventoryList)
    {
        if (inventoryLocation == InventoryLocation.player)
        {
            InitializeInventoryManagementSlots();

            // For each of the player's inventory items
            for (int i = 0; i < InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player].Count; i++)
            {
                // Find the item's item details
                inventoryManagementSlot[i].itemDetails = InventoryManager.Instance.GetItemDetails(playerInventoryList[i].itemCode);
                inventoryManagementSlot[i].itemQuantity = playerInventoryList[i].itemQuantity;

                // Check that we found the item details
                if (inventoryManagementSlot[i].itemDetails != null)
                {
                    // If so, update the inventory with the item's image and amount
                    inventoryManagementSlot[i].inventoryManagementSlotImage.sprite = inventoryManagementSlot[i].itemDetails.itemSprite;
                    inventoryManagementSlot[i].textMeshProUGUI.text = inventoryManagementSlot[i].itemQuantity.ToString();
                }
            }
        }
    }


    private void InitializeInventoryManagementSlots()
    {
        // Loop through each inventory slot
        for (int i = 0; i < Settings.playerMaximumInventoryCapacity; i++)
        {
            // Clear the inventory slot
            inventoryManagementSlot[i].greyedOutImageGO.SetActive(false);
            inventoryManagementSlot[i].itemDetails = null;
            inventoryManagementSlot[i].itemQuantity = 0;
            inventoryManagementSlot[i].inventoryManagementSlotImage.sprite = transparent16x16;
            inventoryManagementSlot[i].textMeshProUGUI.text = "";
        }

        // Based on the player's maximum inventory slots, loop through the slots the user should not have access to and set the grayed out image
        for (int i = InventoryManager.Instance.inventoryListCapacityIntArray[(int)InventoryLocation.player]; i < Settings.playerMaximumInventoryCapacity; i++)
        {
            inventoryManagementSlot[i].greyedOutImageGO.SetActive(true);
        }
    }
}
