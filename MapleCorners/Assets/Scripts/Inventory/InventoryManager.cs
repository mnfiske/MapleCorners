// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections.Generic;
using UnityEngine;

// Inherits from Abstract class of game object
public class InventoryManager : SingletonMonoBehavior<InventoryManager>, ISaveable
{
    private UIInventoryBar inventoryBar;
    
    // dictionary to hold items in scriptable object list
    private Dictionary<int, ItemDetails> itemDetailsDictionary;

    public List<InventoryItem>[] inventoryLists;

    // An array of item codes representing the inventory
    private int[] selectedInventoryItem;

    [HideInInspector] public int[] inventoryListCapacityIntArray;

    [SerializeField] private SO_ItemList itemList = null;

    private string _iSaveableID;
    public string ISaveableID { get { return _iSaveableID; } set { _iSaveableID = value; } }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    // awake occurs before start, ensures that Item Details Dictionary will be created first
    protected override void Awake()
    {
        base.Awake(); //ensures that awake in singletonmono still runs

        CreateInventoryLists();

        // create dictionary
        CreateItemDetailsDictionary();

        // Create new array for selected item
        selectedInventoryItem = new int[(int)InventoryLocation.count];

        for (int i = 0; i < selectedInventoryItem.Length; i++)
        {
            selectedInventoryItem[i] = -1;
        }

        // Get a guid and store it in the ISaveableID
        ISaveableID = GetComponent<GenerateGuid>().Guid;

        GameObjectSave = new GameObjectSave();
    }

    private void OnEnable()
    {
        ISaveableRegister();
    }

    private void OnDisable()
    {
        ISaveableDeregister();
    }

    private void Start()
    {
        inventoryBar = FindObjectOfType<UIInventoryBar>();
    }

    private void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for (int i = 0; i < (int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }

        inventoryListCapacityIntArray = new int[(int)InventoryLocation.count];

        inventoryListCapacityIntArray[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
    }

    /// <summary>
    /// Populates the itemDetailsDictionary from the scriptable object items list
    /// </summary>
    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();

        // loop through scriptable objects to populate
        foreach (ItemDetails itemDetails in itemList.itemDetails)
        {
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }
    }

    public void SwapInventoryItems(InventoryLocation inventoryLocation, int fromItem, int toItem)
    {
        // if they can be swapped
        if (fromItem < inventoryLists[(int)inventoryLocation].Count && toItem < inventoryLists[(int)inventoryLocation].Count
             && fromItem != toItem && fromItem >= 0 && toItem >= 0)
        {
            InventoryItem fromInventoryItem = inventoryLists[(int)inventoryLocation][fromItem];
            InventoryItem toInventoryItem = inventoryLists[(int)inventoryLocation][toItem];

            inventoryLists[(int)inventoryLocation][toItem] = fromInventoryItem;
            inventoryLists[(int)inventoryLocation][fromItem] = toInventoryItem;

            // send inventory update event
            EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
        }
    }

    public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        selectedInventoryItem[(int)inventoryLocation] = -1;
    }

    /// Add an item to the inventory list for the inventoryLocation and then destroy the gameObjectToDelete
    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameObjectToDelete)
    {
        AddItem(inventoryLocation, item);

        Destroy(gameObjectToDelete);
    }

    /// Add an item to the inventory list without destroying item as the same item is used for multiple purchases
    public void AddBoughtItem(InventoryLocation inventoryLocation, Item item, GameObject gameObjectToDelete)
    {
        AddItem(inventoryLocation, item);
    }

    /// Add an item to the inventory list for the inventoryLocation
    public void AddItem(InventoryLocation inventoryLocation, Item item)
    {
        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        // Check if item is already in inventory
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }

        //  Send event that inventory has been updated
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }

    public void AddItem(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        // Check if item is already in inventory
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }

        //  Send event that inventory has been updated
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }

    /// Checks inventory for item. Returns the item position
    /// or -1 if the item is not in the inventory
    public int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].itemCode == itemCode)
            {
                return i;
            }
        }

        return -1;
    }

    /// Add item to the end of the inventory
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem();

        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryList.Add(inventoryItem);

        DebugPrintInventoryList(inventoryList);
    }

    /// Add item to position in the inventory
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[position].itemQuantity + 1;
        inventoryItem.itemQuantity = quantity;
        inventoryItem.itemCode = itemCode;
        inventoryList[position] = inventoryItem;
    }

    /// <summary
    /// Returns the itemDetails for the itemCode or null
    /// </sumary>
    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetails;

        // see if item exists in dictionary
        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
        {
            return itemDetails;
        }
        else
        {
            return null;
        }
    }


    // get the item type name in order to populate the text box
    public string GetItemTypeDescription(ItemType itemType)
    {
        string itemTypeDescription;
        switch (itemType)
        {
            case ItemType.Breaking_tool:
                itemTypeDescription = Settings.BreakingTool;
                break;

            case ItemType.Chopping_tool:
                itemTypeDescription = Settings.ChoppingTool;
                break;

            case ItemType.Hoeing_tool:
                itemTypeDescription = Settings.HoeingTool;
                break;

            case ItemType.Reaping_tool:
                itemTypeDescription = Settings.ReapingTool;
                break;

            case ItemType.Watering_tool:
                itemTypeDescription = Settings.WateringTool;
                break;

            case ItemType.Collecting_tool:
                itemTypeDescription = Settings.CollectingTool;
                break;

            default:
                itemTypeDescription = itemType.ToString();
                break;
        }

        return itemTypeDescription;
    }

    /// <summary>
    /// Returns the item code for the item at the passed in inventory location
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <returns></returns>
    private int GetSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
      return selectedInventoryItem[(int)inventoryLocation];
    }

    /// <summary>
    /// Returns the ItemDetails for the item at the passed in inventory location
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <returns></returns>
    public ItemDetails GetSelectedInventoryItemDetails(InventoryLocation inventoryLocation)
    {
      int itemCode = GetSelectedInventoryItem(inventoryLocation);

      if (itemCode == -1)
      {
        return null;
      }
      else
      {
        return GetItemDetails(itemCode);
      }
    }

  public void RemoveItem(InventoryLocation inventoryLocation, int itemCode)

    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        // if inventory contains the item
        if (itemPosition != -1)
        {
            RemoveItemAtPosition(inventoryList, itemCode, itemPosition);
        }

        //  update inventory event
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);

    }

    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[position].itemQuantity - 1;

        if (quantity > 0)
        {
            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemCode = itemCode;
            inventoryList[position] = inventoryItem;
        }
        else
        {
            inventoryList.RemoveAt(position);
        }
    }

    public void SetSelectedInventoryItem(InventoryLocation inventoryLocation, int itemCode)
    {
        selectedInventoryItem[(int)inventoryLocation] = itemCode;
    }

    /// <summary>
    /// Register the inventory with ISaveable
    /// </summary>
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjects.Add(this);
    }

    /// <summary>
    /// Deregister the inventory with ISaveable
    /// </summary>
    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjects.Remove(this);
    }

    public GameObjectSave ISaveableSave()
    {
        // Remove existing save data for this scene if it exists
        GameObjectSave.SceneData.Remove(Settings.MainScene);

        // Store inventoryLists in the SceneSave object
        SceneSave sceneSave = new SceneSave();
        sceneSave.ListInvItemArray = inventoryLists;

        // Save the maximum capacities of the invventory
        sceneSave.intArrayDictionary = new Dictionary<string, int[]>();
        sceneSave.intArrayDictionary.Add("inventoryListCapacityArray", inventoryListCapacityIntArray);

        // Add the scene data to the GameObjectSave
        GameObjectSave.SceneData.Add(Settings.MainScene, sceneSave);

        return GameObjectSave;
    }

    public void ISaveableLoad(GameSave gameSave)
    {
        // Check the save data for the game object
        if (gameSave.gameObjectData.TryGetValue(ISaveableID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;

            // Get the data from the main scene--this is where the inventory is stored
            if (gameObjectSave.SceneData.TryGetValue(Settings.MainScene, out SceneSave sceneSave))
            {
                // Check if we have our inventory items list
                if (sceneSave.ListInvItemArray != null)
                {
                    inventoryLists = sceneSave.ListInvItemArray;

                    //  Update the inventory
                    for (int i = 0; i < (int)InventoryLocation.count; i++)
                    {
                        EventHandler.CallInventoryUpdatedEvent((InventoryLocation)i, inventoryLists[i]);
                    }

                    // Clear any carried items or highlights
                    Player.Instance.ClearCarriedItem();
                    inventoryBar.ClearHighlightOnInventorySlots();
                }

                // Check for the inventoryListCapacityIntArray
                if (sceneSave.intArrayDictionary != null && sceneSave.intArrayDictionary.TryGetValue("inventoryListCapacityArray", out int[] inventoryCapacityArray))
                {
                    // Set the inventoryListCapacityIntArray
                    inventoryListCapacityIntArray = inventoryCapacityArray;
                }
            }

        }
    }

    public void ISaveableStoreScene(string sceneName)
    {
        // No scene data to store
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        // No scene data to restore
    }

    //print inventory items to console for debugging
    //commented out now that invetory bar UI updates with items that are picked up
    private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    {
        foreach (InventoryItem inventoryItem in inventoryList)
        {
            Debug.Log("Item Description:" + InventoryManager.Instance.GetItemDetails(inventoryItem.itemCode).itemDescription + "    Item Quantity: " + inventoryItem.itemQuantity);
        }
        Debug.Log("****************************************************************");
    }

}
