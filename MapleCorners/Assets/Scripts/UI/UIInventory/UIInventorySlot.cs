// "Citation: Unity 2D Game Developer Course Farming RPG"

using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera mainCamera;
    private Transform parentItem;
    private GridCursor gridCursor;
    private GameObject draggedItem;
    private Canvas parentCanvas;

    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private UIInventoryBar inventoryBar = null;
    [SerializeField] private GameObject itemPrefab = null;
    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        gridCursor = FindObjectOfType<GridCursor>();
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SceneLoaded;
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SceneLoaded;
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
    if (itemDetails != null)
    {
      Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

      // Create item from prefab
      GameObject itemGameObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
      Item item = itemGameObject.GetComponent<Item>();
      item.ItemCode = itemDetails.itemCode;

      // Remove item from players inventory
      InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);

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

      gridCursor.SelectedItemType = ItemType.none;
    }
}
