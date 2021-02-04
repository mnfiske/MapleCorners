using UnityEngine;

// can include in scriptable objects
[System.Serializable]
public class ItemDetails
{
    // Variables used for all items
    public int itemCode;
    public ItemType itemType;
    public string itemDescription;
    public Sprite itemSprite;
    public string itemLongDescription;
    // some items can only be done with x grid spaces from player
    public short itemUseGridRadius;
    // may be based on fixed space from player rathe than grid
    public float itemUseRadius;
    public bool isStartingItem;
    public bool canBePickedUp;
    public bool canBeDropped;
    public bool canBeEaten;
    public bool canBeCarried;
}
