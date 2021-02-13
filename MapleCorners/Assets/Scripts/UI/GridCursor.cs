// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCursor : MonoBehaviour
{
  // Store the UI canvas the cursor is in
  private Canvas canvas;
  // Grid field for the tilemaps
  private Grid grid;
  // Holds the reference to the main camera
  private Camera mainCamera;
  // The image component on the cursor game object
  [SerializeField] private Image cursorImage = null;
  // The RectTransform on the cursor game object
  [SerializeField] private RectTransform cursorRectTransform = null;
  // A reference to the green cursor sprite
  [SerializeField] private Sprite greenCursorSprite = null;
  // A reference to the red cursor sprite
  [SerializeField] private Sprite redCursorSprite = null;

  // Whether the cursor position is valid or not
  private bool _cursorPositionIsValid = false;
  public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }

  // The grid radius for the selected item
  private int _itemUseGridRadius = 0;
  public int ItemUseGridRadius { get => _itemUseGridRadius; set => _itemUseGridRadius = value; }

  // The ItemType of the selected item
  private ItemType _selectedItemType;
  public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }

  // Whether the cursor is enabled
  private bool _cursorIsEnabled = false;
  public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value; }


  /// <summary>
  /// Subscribes the AfterSceneLoadEvent to the SceneLoaded method
  /// </summary>
  private void OnEnable()
  {
    EventHandler.AfterSceneLoadEvent += SceneLoaded;
  }

  /// <summary>
  /// Unsubscribes the AfterSceneLoadEvent from the SceneLoaded method
  /// </summary>
  private void OnDisable()
  {
    EventHandler.AfterSceneLoadEvent -= SceneLoaded;
  }

  /// <summary>
  /// Populates the grid member variable by finding the grid object
  /// </summary>
  private void SceneLoaded()
  {
    grid = GameObject.FindObjectOfType<Grid>();
  }

  /// <summary>
  /// Cache the main camera and canvas
  /// </summary>
  private void Start()
  {
    mainCamera = Camera.main;
    canvas = GetComponentInParent<Canvas>();
  }

  /// <summary>
  /// Displays the cursor if it is enabled
  /// </summary>
  private void Update()
  {
    if (CursorIsEnabled)
    {
      DisplayCursor();
    }
  }

  /// <summary>
  /// Returns the grid position of the cursor
  /// </summary>
  /// <returns></returns>
  public Vector3Int GetGridPositionForCursor()
  {
    // Convert the mouse position from a screen point to a world point
    Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));  // z is how far the objects are in front of the camera - camera is at -10 so objects are (-)-10 in front = 10
    // Conver the world position into a cell position & return it
    return grid.WorldToCell(worldPosition);
  }

  /// <summary>
  /// Retuurns the grid position of the player
  /// </summary>
  /// <returns></returns>
  public Vector3Int GetGridPositionForPlayer()
  {
    // Convert the world position of the player into a cell position & return it
    return grid.WorldToCell(Player.Instance.transform.position);
  }

  /// <summary>
  /// Returns the pixel position of the grid position passed in
  /// </summary>
  /// <param name="gridPosition"></param>
  /// <returns></returns>
  public Vector2 GetRectTransformPositionForCursor(Vector3Int gridPosition)
  {
    Vector3 gridWorldPosition = grid.CellToWorld(gridPosition);
    Vector2 gridScreenPosition = mainCamera.WorldToScreenPoint(gridWorldPosition);
    return RectTransformUtility.PixelAdjustPoint(gridScreenPosition, cursorRectTransform, canvas);
  }

  /// <summary>
  /// Displays the cursor
  /// </summary>
  /// <returns></returns>
  private Vector3Int DisplayCursor()
  {
    if (grid == null)
    {
      return Vector3Int.zero;
    }
    else
    {
      // Get grid position for cursor
      Vector3Int gridPosition = GetGridPositionForCursor();

      // Get grid position for player
      Vector3Int playerGridPosition = GetGridPositionForPlayer();

      // Set cursor sprite
      SetCursorValidity(gridPosition, playerGridPosition);

      // Get rect transform position for cursor
      cursorRectTransform.position = GetRectTransformPositionForCursor(gridPosition);

      return gridPosition;
    }
  }

  /// <summary>
  /// Sets the cursor sprite to red and sets that the cursor position is invalid
  /// </summary>
  private void SetCursorToInvalid()
  {
    cursorImage.sprite = redCursorSprite;
    CursorPositionIsValid = false;
  }

  /// <summary>
  /// Sets the cursor sprite to green and sets that the cursor position is valid
  /// </summary>
  private void SetCursorToValid()
  {
    cursorImage.sprite = greenCursorSprite;
    CursorPositionIsValid = true;
  }

  /// <summary>
  /// Returns whether an item can be dropped at the passeed in gridPropertyDetails
  /// </summary>
  /// <param name="gridPropertyDetails"></param>
  /// <returns></returns>
  private bool CursorCanDropItem(GridPropertyDetails gridPropertyDetails)
  {
    return gridPropertyDetails.CanDropItem;
  }

  /// <summary>
  /// Set whether the cursor position is valid
  /// </summary>
  /// <param name="cursorGridPosition"></param>
  /// <param name="playerGridPosition"></param>
  private void SetCursorValidity(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
  {
    // Start with the cursor position valid
    SetCursorToValid();

    // Determine whether the position is within the item's use radius
    if (Mathf.Abs(cursorGridPosition.x - playerGridPosition.x) > ItemUseGridRadius
        || Mathf.Abs(cursorGridPosition.y - playerGridPosition.y) > ItemUseGridRadius)
    {
      // If we've reached this line, the position is not within the item's use radius, set the cursor to invalid and return
      SetCursorToInvalid();
      return;
    }

    // Get the details of the selected item
    ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

    // Check if the selected item details are null
    if (itemDetails == null)
    {
      // If so, that means we don't have an item selected. Set the cursor to invalid and return
      SetCursorToInvalid();
      return;
    }

    // Get the GetGridPropertyDetails at the cursor position
    GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

    if (gridPropertyDetails == null)
    {
      // If the GetGridPropertyDetails are null, set the cursor to invalid and return
      SetCursorToInvalid();
      return;
    }
    else
    {
      // Test if the cursor position is valid, based on the item type of the selected item
      switch (itemDetails.itemType)
      {
        case ItemType.Seed:
        case ItemType.Commodity:
          if (!CursorCanDropItem(gridPropertyDetails))
          {
            SetCursorToInvalid();
            return;
          }
          break;

        case ItemType.none:
          break;

        case ItemType.count:
          break;

        default:
          break;
      }
    }
  }

  /// <summary>
  /// Sets the cursor image to not be transparent & sets CursorIsEnabled to true
  /// </summary>
  public void EnableCursor()
  {
    cursorImage.color = new Color(1f, 1f, 1f, 1f);
    CursorIsEnabled = true;
  }

  /// <summary>
  /// Sets the cursor image to transparent & sets CursorIsEnabled to false
  /// </summary>
  public void DisableCursor()
  {
    cursorImage.color = Color.clear;

    CursorIsEnabled = false;
  }
  
}
