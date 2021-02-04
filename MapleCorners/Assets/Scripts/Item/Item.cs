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
}
