// "Citation: Unity 2D Game Developer Course Farming RPG"

using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Camera mainCamera;
    private Transform parentItem;
    private GridCursor gridCursor;
    private Cursor cursor;
    private GameObject draggedItem;
    private Canvas parentCanvas;

    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private UIInventoryBar inventoryBar = null;
    [SerializeField] private GameObject inventoryTextboxPrefab = null;
    [SerializeField] private GameObject itemPrefab = null;
    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;
    [HideInInspector] public bool isSelected = false;

    [SerializeField] private int slotNumber = 0;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        gridCursor = FindObjectOfType<GridCursor>();
        cursor = FindObjectOfType<Cursor>();
    }

    private void SetSelectedItem()
    {
        // Clear currently highlighted items
        inventoryBar.ClearHighlightOnInventorySlots();

        // Highlight item on inventory bar
        isSelected = true;

        // Set highlighted inventory slots
        inventoryBar.SetHighlightedInventorySlots();

        // Store the item's use radius for the cursors
        gridCursor.ItemUseGridRadius = itemDetails.itemUseGridRadius;
        cursor.ItemUseRadius = itemDetails.itemUseRadius;

        // If the item's itemUseGridRadius is greater than 0 we want to enable the grid cursor, else disable it
        if (itemDetails.itemUseGridRadius > 0)
        {
            gridCursor.EnableCursor();
        }
        else
        {
            gridCursor.DisableCursor();
        }

        // If the item's itemUseRadius is greater than 0 we want to enable the grid cursor, else disable it
        if (itemDetails.itemUseRadius > 0f)
        {
            cursor.EnableCursor();
        }
        else
        {
            cursor.DisableCursor();
        }


        // Set the cursors' SelectedItemType to the item's itemType
        gridCursor.SelectedItemType = itemDetails.itemType;
        cursor.SelectedItemType = itemDetails.itemType;

        // Set item selected in inventory
        InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, itemDetails.itemCode);

        if (itemDetails.canBeCarried == true)
        {
            Player.Instance.ShowCarriedItem(itemDetails.itemCode);
        }
        else
        {
            Player.Instance.ClearCarriedItem();
        }
    }


    public void ClearSelectedItem()
    {
        ClearCursors();

        // Clear currently highlighted items
        inventoryBar.ClearHighlightOnInventorySlots();

        isSelected = false;

        // set no item selected in inventory
        InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);

        Player.Instance.ClearCarriedItem();
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SceneLoaded;
        EventHandler.DropSelectedItemEvent -= DropSelectedItemAtMousePosition;
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SceneLoaded;
        EventHandler.DropSelectedItemEvent += DropSelectedItemAtMousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // if we have items in inventory slot
        if (itemDetails != null)
        {
            // disable player input
            Player.Instance.DisablePlayerInputAndResetMovement();

            // Instatiate gameobject as dragged item
            draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);

            // Get image for item
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;

            SetSelectedItem();

        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            Destroy(draggedItem);

            // if the drag ends over the inventory bar
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
            {
                // get slot number where drag ends
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>().slotNumber;

                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);

                DestroyInventoryTextBox();

                ClearSelectedItem();
            }
            // if the drag ends over the scene
            else
            {
                // drop it if it can be dropped
                if (itemDetails.canBeDropped)
                {
                    DropSelectedItemAtMousePosition();
                }
            }

            // Enable player input
            Player.Instance.EnablePlayerInput();
        }
    }

    private void DropSelectedItemAtMousePosition()
    {
        if (itemDetails != null && isSelected)
        {

            if (gridCursor.CursorPositionIsValid)
            {
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
                
                // Create item from prefab
                GameObject itemGameObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
                Item item = itemGameObject.GetComponent<Item>();
                item.ItemCode = itemDetails.itemCode;

                // Remove item from players inventory
                InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);

                // check if there are any items left, if not clear the selected item
                if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, item.ItemCode) == -1)
                {
                    ClearSelectedItem();
                }
            }
        }
    }
    // looking for left clicks
    public void OnPointerClick(PointerEventData eventData)
    {
        // if left click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // if currently selected then deselect
            if (isSelected == true)
            {
                ClearSelectedItem();
            }
            else
            {
                if (itemQuantity > 0)
                {
                    SetSelectedItem();
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Populate text box with item details
        if (itemQuantity != 0)
        {
            // Instantiate inventory text box
            inventoryBar.inventoryTextBoxGameobject = Instantiate(inventoryTextboxPrefab, transform.position, Quaternion.identity);
            inventoryBar.inventoryTextBoxGameobject.transform.SetParent(parentCanvas.transform, false);

            UIInventoryTextbox inventoryTextBox = inventoryBar.inventoryTextBoxGameobject.GetComponent<UIInventoryTextbox>();

            // Set item type description
            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);

            // Populate text box
            inventoryTextBox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");

            // Set text box position according to inventory bar position
            if (inventoryBar.IsInventoryBarPositionBottom)

            {
                inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);
            }
            else
            {
                inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInventoryTextBox();
    }

    public void DestroyInventoryTextBox()
    {
        if (inventoryBar.inventoryTextBoxGameobject != null)
        {
            Destroy(inventoryBar.inventoryTextBoxGameobject);
        }
    }

  public void SceneLoaded()
    {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
    }

    /// <summary>
    /// Disables the gridCuror & sets its SelectedItemType to none
    /// </summary>
    private void ClearCursors()
    {
        gridCursor.DisableCursor();
        cursor.DisableCursor();

        gridCursor.SelectedItemType = ItemType.none;
        cursor.SelectedItemType = ItemType.none;
    }
}
