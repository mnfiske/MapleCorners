using System.Collections.Generic;
using UnityEngine;

// Inherits from Abstract class of game object
public class InventoryManager : SingletonMonoBehavior<InventoryManager>
{
    // dictionary to hold items in scriptable object list
    private Dictionary<int, ItemDetails> itemDetailsDictionary;

    [SerializeField] private SO_ItemList itemList = null;

    // awake occurs before start, ensures that Item Details Dictionary will be created first
    protected override void Awake()
    {
        base.Awake(); //ensures that awake in singletonmono still runs

        // create dictionary
        CreateItemDetailsDictionary();
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
}
