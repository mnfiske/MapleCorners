 using UnityEngine;

public class Item : MonoBehaviour
{
    [ItemCodeDescription]
    [SerializeField]
    private int _itemCode;

    private SpriteRenderer spriteRenderer;

    // Getter and setter for item based on code
    public int ItemCode { get { return _itemCode; } set { _itemCode = value; } }

    // populate sprite renderer.
    // Item class is attached to a game object
    // will have child with sprite renderer componentn
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren <SpriteRenderer>();
    }

    private void Start()
    {
        if( ItemCode != 0 )
        {
            Init(ItemCode);
        }
    }

    public void Init(int itemCodeParam)
    {
        if (itemCodeParam != 0)
        {
            ItemCode = itemCodeParam;

            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(ItemCode);

            spriteRenderer.sprite = itemDetails.itemSprite;

            // If item type is reapable, add item nudge behavior
            if (itemDetails.itemType == ItemType.Reapable_scenery)
            {
                gameObject.AddComponent<ItemNudge>();
            }
        }
    }

    public void BuyItem ()
    {
        // currently there is only one item of each type in the shop scene so player can only buy one
        // TODO: instantiate new item when one is bought, to allow for multiple purchases

        Item item = GameObject.FindGameObjectWithTag(Tags.shopItem).GetComponent<Item>();
        InventoryManager.Instance.AddItem(InventoryLocation.player, item, GameObject.FindGameObjectWithTag(Tags.shopItem));
    }

    public void SellItem()
    {
        Item item = GameObject.FindGameObjectWithTag(Tags.SellCorn).GetComponent<Item>();
        //Debug.Log(item);
        //if inventory contains that item, remove it
        if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, item.ItemCode) != -1)
        {
            InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);
        }
        else
        {
            // do nothing, no item to sell
        }
    }
}
