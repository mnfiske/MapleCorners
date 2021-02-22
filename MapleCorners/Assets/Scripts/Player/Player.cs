// "Citation: Unity 2D Game Developer Course Farming RPG"

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : SingletonMonoBehavior<Player>
{
    private WaitForSeconds afterUseToolAnimationPause;
    private WaitForSeconds afterLiftToolAnimationPause;
    private WaitForSeconds afterPickAnimationPause;

    private AnimationOverrides animationOverrides;
    private GridCursor gridCursor;
    private Cursor cursor;

    // Movement Parameters from player animation
    private float inputX;
    private float inputY;
    private bool isWalking;
    private bool isRunning;
    private bool isIdle;
    private bool isCarrying = false;
    private ToolEffect toolEffect =ToolEffect.none;
    private bool isUsingToolRight;
    private bool isUsingToolLeft;
    private bool isUsingToolUp;
    private bool isUsingToolDown;
    private bool isLiftingToolRight;
    private bool isLiftingToolLeft;
    private bool isLiftingToolUp;
    private bool isLiftingToolDown;
    private bool isPickingRight;
    private bool isPickingLeft;
    private bool isPickingUp;
    private bool isPickingDown;
    private bool isSwingingToolRight;
    private bool isSwingingToolLeft;
    private bool isSwingingToolUp;
    private bool isSwingingToolDown;
    private bool idleRight;
    private bool idleLeft;
    private bool idleUp;
    private bool idleDown;

    private Camera mainCamera;

    private bool playerToolUseDisabled = false;

    private WaitForSeconds useToolAnimationPause;
    private WaitForSeconds liftToolAnimationPause;
    private WaitForSeconds pickAnimationPause;

    private Rigidbody2D rigidBody2D;

    private Direction playerDirection;

    private List<CharacterAttribute> characterAttributeCustomizationList;
    private float movementSpeed;

    [Tooltip("Should be populated in the prefab with the equipped item sprite renderer")]
    [SerializeField] private SpriteRenderer equippedItemSpriteRenderer = null;

    private CharacterAttribute armsCharacterAttribute;

    private CharacterAttribute toolCharacterAttribute;

    private bool _playerInputIsDisabled = false;

    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    protected override void Awake()
    {
        //calls the absctract class it inherits from
        base.Awake();

        rigidBody2D = GetComponent<Rigidbody2D>();

        animationOverrides = GetComponentInChildren<AnimationOverrides>();

        armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.arms, PartVariantColour.none, PartVariantType.none);
        toolCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.tool, PartVariantColour.none, PartVariantType.hoe);

        characterAttributeCustomizationList = new List<CharacterAttribute>();

        //references the main camera
        mainCamera = Camera.main;
    }


    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadFadeOutEvent += DisablePlayerInputAndResetMovement;
        EventHandler.AfterSceneLoadFadeInEvent += EnablePlayerInput;
    }


    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadFadeOutEvent -= DisablePlayerInputAndResetMovement;
        EventHandler.AfterSceneLoadFadeInEvent -= EnablePlayerInput;
    }

    private void Start()
    {
        gridCursor = FindObjectOfType<GridCursor>();
        cursor = FindObjectOfType<Cursor>();

        useToolAnimationPause = new WaitForSeconds(Settings.useToolAnimationPause);
        afterUseToolAnimationPause = new WaitForSeconds(Settings.afterUseToolAnimationPause);
        liftToolAnimationPause = new WaitForSeconds(Settings.liftToolAnimationPause);
        afterLiftToolAnimationPause = new WaitForSeconds(Settings.afterLiftToolAnimationPause);
        pickAnimationPause = new WaitForSeconds(Settings.pickAnimationPause);
        afterPickAnimationPause = new WaitForSeconds(Settings.afterPickAnimationPause);
    }

  private void Update()
    {
        #region Player Input

        // player input is only recognized if player input is not disabled
        if (!PlayerInputIsDisabled)
        {
            ResetAnimationTriggers();

            PlayerMovementInput();

            PlayerWalkInput();

            PlayerClickInput();

            //Remove!
            PlayerTestInput();

            // Trigger event
            EventHandler.CallMovementEvent(inputX, inputY, isWalking, isRunning, isIdle, isCarrying, toolEffect,
                    isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                    isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                    isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                    isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                    false, false, false, false);
        }

        #endregion

    }

    private void FixedUpdate()
    {
        // move player, not animation
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector2 move = new Vector2(inputX * movementSpeed * Time.deltaTime, inputY * movementSpeed * Time.deltaTime);

        rigidBody2D.MovePosition(rigidBody2D.position + move);
    }

    private void ResetAnimationTriggers()
    {
        isPickingRight = false;
        isPickingLeft = false;
        isPickingUp = false;
        isPickingDown = false;
        isUsingToolRight = false;
        isUsingToolLeft = false;
        isUsingToolUp = false;
        isUsingToolDown = false;
        isLiftingToolRight = false;
        isLiftingToolLeft = false;
        isLiftingToolUp = false;
        isLiftingToolDown = false;
        isSwingingToolRight = false;
        isSwingingToolLeft = false;
        isSwingingToolUp = false;
        isSwingingToolDown = false;
        toolEffect = ToolEffect.none;
    }

    private void PlayerMovementInput()
    {
        inputY = Input.GetAxisRaw("Vertical");
        inputX = Input.GetAxisRaw("Horizontal");

        // if player is pressing both up/down and left/right, move diagonally
        if (inputY != 0 && inputX != 0)
        {
            inputX = inputX * 0.71f;
            inputY = inputY * 0.71f;
        }

        //if player is pressing left/right/up/down key
        if (inputX != 0 || inputY != 0)
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;

            // Capture player direction for save game
            if (inputX < 0)
            {
                playerDirection = Direction.left;
            }
            else if (inputX > 0)
            {
                playerDirection = Direction.right;
            }
            else if (inputY < 0)
            {
                playerDirection = Direction.down;
            }
            else
            {
                playerDirection = Direction.up;
            }
        }
        else if (inputX == 0 && inputY == 0)
        {
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }
    }

    private void PlayerWalkInput()
    {
        // if shift keys are being pressed, player is walking
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isRunning = false;
            isWalking = true;
            isIdle = false;
            movementSpeed = Settings.walkingSpeed;
        }
        //otherwise default to running
        else
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
        }
    }


    private void PlayerClickInput()
    {
        if (!playerToolUseDisabled)
        {
            if (Input.GetMouseButton(0))
            {
                if (gridCursor.CursorIsEnabled || cursor.CursorIsEnabled)
                {
                    Vector3Int cursorGridPosition = gridCursor.GetGridPositionForCursor();
                    Vector3Int playerGridPosition = gridCursor.GetGridPositionForPlayer();
                    ProcessPlayerClickInput(cursorGridPosition, playerGridPosition);
                }
            }
        }
    }

    private void ProcessPlayerClickInput(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        ResetMovement();

        Vector3Int playerDirection = GetPlayerClickDirection(cursorGridPosition, playerGridPosition);

        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        if (itemDetails != null)
        {
            switch (itemDetails.itemType)
            {
                case ItemType.Seed:
                if (Input.GetMouseButtonDown(0))
                {
                    ProcessPlayerClickInputSeed(itemDetails, gridPropertyDetails);
                }
                break;

                case ItemType.Commodity:
                if (Input.GetMouseButtonDown(0))
                {
                    ProcessPlayerClickInputCommodity(itemDetails);
                }
                break;

                case ItemType.Hoeing_tool:
                case ItemType.Watering_tool:
                case ItemType.Reaping_tool:
                case ItemType.Collecting_tool:
                    ProcessPlayerClickInputTool(gridPropertyDetails, itemDetails, playerDirection);
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

    private void ProcessPlayerClickInputSeed(ItemDetails itemDetails, GridPropertyDetails gridPropertyDetails)
    {
        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid && gridPropertyDetails.DaysSinceDug > -1 && gridPropertyDetails.SeedItemCode == -1)
        {
            PlantSeedAtCursor(gridPropertyDetails, itemDetails);
        }
        else if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
        {
            EventHandler.CallDropSelectedItemEvent();
        }
    }


    private void PlantSeedAtCursor(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {
        // Set the SeedItemCode to the itemCode for the seeds being planted
        gridPropertyDetails.SeedItemCode = itemDetails.itemCode;
        // Set GrowthDays to 0
        gridPropertyDetails.GrowthDays = 0;

        // Display the planted crop
        GridPropertiesManager.Instance.DisplayPlantedCrop(gridPropertyDetails);

        // Remove the seeds from the inventory
        EventHandler.CallRemoveSelectedItemFromInventoryEvent();
    }

    private void ProcessPlayerClickInputCommodity(ItemDetails itemDetails)
    {
        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
        {
            EventHandler.CallDropSelectedItemEvent();
        }
    }

    private void ProcessPlayerClickInputTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerDirection)
    {
        // Switch on tool
        switch (itemDetails.itemType)
        {
            case ItemType.Hoeing_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    HoeGroundAtCursor(gridPropertyDetails, playerDirection);
                }
                break;
            
            case ItemType.Watering_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    WaterGroundAtCursor(gridPropertyDetails, playerDirection);
                }
                break;

            case ItemType.Reaping_tool:
                if (cursor.CursorPositionIsValid)
                {
                    playerDirection = GetPlayerDirection(cursor.GetWorldPositionForCursor(), GetPlayerCenterPosition());
                    ReapInPlayerDirectionAtCursor(itemDetails, playerDirection);
                }
                break;
            case ItemType.Collecting_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    CollectInPlayerDirection(gridPropertyDetails, itemDetails, playerDirection);
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Triggers a coroutine to start the hoe animation & updates the tiles display
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="playerDirection"></param>
    private void HoeGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    {
        StartCoroutine(HoeGroundAtCursorRoutine(playerDirection, gridPropertyDetails));

        GridPropertiesManager.Instance.DisplayDugGround(gridPropertyDetails);
    }

    /// <summary>
    /// Handles hoe animation & updates the grid property details
    /// </summary>
    /// <param name="playerDirection"></param>
    /// <param name="gridPropertyDetails"></param>
    /// <returns></returns>
    private IEnumerator HoeGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails)
    {
        // Disable the player input and tool use
        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        // Switch the animation
        toolCharacterAttribute.partVariantType = PartVariantType.hoe;
        characterAttributeCustomizationList.Clear();
        characterAttributeCustomizationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomizationList);

        // Set the direction in which the character is using the hoe
        if (playerDirection == Vector3Int.right)
        {
            isUsingToolRight = true;
        }
        else if (playerDirection == Vector3Int.left)
        {
            isUsingToolLeft = true;
        }
        else if (playerDirection == Vector3Int.up)
        {
            isUsingToolUp = true;
        }
        else if (playerDirection == Vector3Int.down)
        {
            isUsingToolDown = true;
        }

        //Pause
        yield return useToolAnimationPause;

        // If this ground wasn't already previously dug, set its DaysSinceDug to 0
        if (gridPropertyDetails.DaysSinceDug == -1)
        {
            gridPropertyDetails.DaysSinceDug = 0;
        }

        // Update the GridPropertyDetails for this grid location
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.GridX, gridPropertyDetails.GridY, gridPropertyDetails);

        // Update the tile display
        GridPropertiesManager.Instance.DisplayDugGround(gridPropertyDetails);
        
        // Pause
        yield return afterUseToolAnimationPause;

        // Reenable player input and player tool use
        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }

    /// <summary>
    /// Triggers a coroutine to start the watering animation
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="playerDirection"></param>
    private void WaterGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    {
        StartCoroutine(WaterGroundAtCursorRoutine(playerDirection, gridPropertyDetails));
    }

    /// <summary>
    /// Handles watering animation & updates the grid property details
    /// </summary>
    /// <param name="playerDirection"></param>
    /// <param name="gridPropertyDetails"></param>
    /// <returns></returns>
    private IEnumerator WaterGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails)
    {
        // Disable the player input and tool use
        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        // Switch the animation
        toolCharacterAttribute.partVariantType = PartVariantType.wateringCan;
        characterAttributeCustomizationList.Clear();
        characterAttributeCustomizationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomizationList);

        // Update the toolEffect to watering
        toolEffect = ToolEffect.watering;

        // Set the direction in which the character is using the watering can
        if (playerDirection == Vector3Int.right)
        {
            isLiftingToolRight = true;
        }
        else if (playerDirection == Vector3Int.left)
        {
            isLiftingToolLeft = true;
        }
        else if (playerDirection == Vector3Int.up)
        {
            isLiftingToolUp = true;
        }
        else if (playerDirection == Vector3Int.down)
        {
            isLiftingToolDown = true;
        }

        // Pause
        yield return liftToolAnimationPause;

        // If this ground wasn't already previously watered set its DaysSinceWaered to 0
        if (gridPropertyDetails.DaysSinceWatered == -1)
        {
            gridPropertyDetails.DaysSinceWatered = 0;
        }

        // Update the GridPropertyDetails for this grid location
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.GridX, gridPropertyDetails.GridY, gridPropertyDetails);

        // Update the tile display
        GridPropertiesManager.Instance.DisplayWateredGround(gridPropertyDetails);

        // Pause
        yield return afterLiftToolAnimationPause;

        // Reenable player input and player tool use
        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }

    /// <summary>
    /// Start the CollectInPlayerDirectionRoutine Coroutine 
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="equippedItemDetails"></param>
    /// <param name="playerDirection"></param>
    private void CollectInPlayerDirection(GridPropertyDetails gridPropertyDetails, ItemDetails equippedItemDetails, Vector3Int playerDirection)
    {
        StartCoroutine(CollectInPlayerDirectionRoutine(gridPropertyDetails, equippedItemDetails, playerDirection));
    }

    private IEnumerator CollectInPlayerDirectionRoutine(GridPropertyDetails gridPropertyDetails, ItemDetails equippedItemDetails, Vector3Int playerDirection)
    {
        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        ProcessCropWithEquippedItemInPlayerDirection(playerDirection, equippedItemDetails, gridPropertyDetails);

        yield return pickAnimationPause;

        yield return afterPickAnimationPause;

        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }

    public void ResetMovement()
      {
          inputX = 0f;
          inputY = 0f;
          isRunning = false;
          isWalking = false;
          isIdle = true;
      }

    public void DisablePlayerInputAndResetMovement()
    {
        DisablePlayerInput();
        ResetMovement();

        // Send event to any listeners for player movement input
        EventHandler.CallMovementEvent(inputX, inputY, isWalking, isRunning, isIdle, isCarrying, toolEffect,
            isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
            isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
            isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
             isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
             false, false, false, false);
    }

    public void DisablePlayerInput()
    {
        PlayerInputIsDisabled = true;
    }

    public void EnablePlayerInput()
    {
        PlayerInputIsDisabled = false;
    }

    public void ClearCarriedItem()
    {
        equippedItemSpriteRenderer.sprite = null;
        equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        // Apply base arms 
        armsCharacterAttribute.partVariantType = PartVariantType.none;
        characterAttributeCustomizationList.Clear();
        characterAttributeCustomizationList.Add(armsCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomizationList);

        isCarrying = false;
    }

    public void ShowCarriedItem(int itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if (itemDetails != null)
        {
            // make sprite visible
            equippedItemSpriteRenderer.sprite = itemDetails.itemSprite;
            equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);

            // apply 'carry' arms 
            armsCharacterAttribute.partVariantType = PartVariantType.carry;
            characterAttributeCustomizationList.Clear();
            characterAttributeCustomizationList.Add(armsCharacterAttribute);
            animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomizationList);

            isCarrying = true;
        }
    }


    public Vector3 GetPlayerViewportPosition()
    {
        return mainCamera.WorldToViewportPoint(transform.position);
    }

    /// <summary>
    /// Returns the direction, relative to the player, of the cursor on click
    /// </summary>
    /// <param name="cursorGridPosition"></param>
    /// <param name="playerGridPosition"></param>
    /// <returns></returns>
    private Vector3Int GetPlayerClickDirection(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        if (cursorGridPosition.x > playerGridPosition.x)
        {
            return Vector3Int.right;
        }
        else if (cursorGridPosition.x < playerGridPosition.x)
        {
            return Vector3Int.left;
        }
        else if (cursorGridPosition.y > playerGridPosition.y)
        {
            return Vector3Int.up;
        }
        else
        {
            return Vector3Int.down;
        }
    }

    /// <summary>
    /// Returns which direction the tool should be used in
    /// </summary>
    /// <param name="cursorPosition"></param>
    /// <param name="playerPosition"></param>
    /// <returns></returns>
    private Vector3Int GetPlayerDirection(Vector3 cursorPosition, Vector3 playerPosition)
    {
        if (cursorPosition.x > playerPosition.x &&
            cursorPosition.y < (playerPosition.y + cursor.ItemUseRadius / 2f) &&
            cursorPosition.y > (playerPosition.y - cursor.ItemUseRadius / 2f))
        {
            return Vector3Int.right;
        }
        else if (cursorPosition.x < playerPosition.x &&
                 cursorPosition.y < (playerPosition.y + cursor.ItemUseRadius / 2f) &&
                 cursorPosition.y > (playerPosition.y - cursor.ItemUseRadius / 2f))
        {
            return Vector3Int.left;
        }
        else if (cursorPosition.y > playerPosition.y)
        {
            return Vector3Int.up;
        }
        else
        {
            return Vector3Int.down;
        }
    }

    /// <summary>
    /// Returns the center position of the player
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerCenterPosition()
    {
        return new Vector3(transform.position.x, transform.position.y + Settings.playerCenterYOffset, transform.position.z);
    }

    private void ReapInPlayerDirectionAtCursor(ItemDetails itemDetails, Vector3Int playerDirection)
    {
        StartCoroutine(ReapInPlayerDirectionAtCursorRoutine(itemDetails, playerDirection));
    }


    private IEnumerator ReapInPlayerDirectionAtCursorRoutine(ItemDetails itemDetails, Vector3Int playerDirection)
    {
        // Disable player input & tool use
        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        // Set the animation
        toolCharacterAttribute.partVariantType = PartVariantType.scythe;
        characterAttributeCustomizationList.Clear();
        characterAttributeCustomizationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomizationList);

        // Reap
        UseToolInPlayerDirection(itemDetails, playerDirection);

        // Pause
        yield return useToolAnimationPause;

        // Enable player input & tool use
        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }

    /// <summary>
    /// Reap
    /// </summary>
    /// <param name="equippedItemDetails"></param>
    /// <param name="playerDirection"></param>
    private void UseToolInPlayerDirection(ItemDetails equippedItemDetails, Vector3Int playerDirection)
    {
        if (Input.GetMouseButton(0))
        {
            switch (equippedItemDetails.itemType)
            {
                case ItemType.Reaping_tool:
                    // Ser the animation direction
                    if (playerDirection == Vector3Int.right)
                    {
                        isSwingingToolRight = true;
                    }
                    else if (playerDirection == Vector3Int.left)
                    {
                        isSwingingToolLeft = true;
                    }
                    else if (playerDirection == Vector3Int.up)
                    {
                        isSwingingToolUp = true;
                    }
                    else if (playerDirection == Vector3Int.down)
                    {
                        isSwingingToolDown = true;
                    }
                    break;
            }

            // Set the center point of the square
            Vector2 point = new Vector2(GetPlayerCenterPosition().x + (playerDirection.x * (equippedItemDetails.itemUseRadius / 2f)), 
                            GetPlayerCenterPosition().y + playerDirection.y * (equippedItemDetails.itemUseRadius / 2f));

            // Set the size of thw square
            Vector2 size = new Vector2(equippedItemDetails.itemUseRadius, equippedItemDetails.itemUseRadius);

            // Get the components of the type Item within the box area
            Item[] itemArray = HelperMethods.GetComponentsAtBoxLocationNonAlloc<Item>(Settings.maxCollidersToTestPerReapSwing, point, size, 0f);

            int reapableItemCount = 0;

            // Loop through all the items
            for (int i = itemArray.Length - 1; i >= 0; i--)
            {
                if (itemArray[i] != null)
                {
                    // Check if the item has a type of Reapable_scenery
                    if (InventoryManager.Instance.GetItemDetails(itemArray[i].ItemCode).itemType == ItemType.Reapable_scenery)
                    {
                        // If so, set the position for the cutting effect
                        Vector3 effectPosition = new Vector3(itemArray[i].transform.position.x, itemArray[i].transform.position.y + Settings.gridCellSize / 2f, itemArray[i].transform.position.z);

                        // Destroy the targeted item
                        Destroy(itemArray[i].gameObject);

                        reapableItemCount++;
                        if (reapableItemCount >= Settings.maxTargetComponentsToDestroyPerReapSwing)
                            break;
                    }
                }
            }
        }
    }

    private void ProcessCropWithEquippedItemInPlayerDirection(Vector3Int playerDirection, ItemDetails equippedItemDetails, GridPropertyDetails gridPropertyDetails)
    {
        switch (equippedItemDetails.itemType)
        {

            case ItemType.Collecting_tool:

                if (playerDirection == Vector3Int.right)
                {
                    isPickingRight = true;
                }
                else if (playerDirection == Vector3Int.left)
                {
                    isPickingLeft = true;
                }
                else if (playerDirection == Vector3Int.up)
                {
                    isPickingUp = true;
                }
                else if (playerDirection == Vector3Int.down)
                {
                    isPickingDown = true;
                }
                break;

            case ItemType.none:
                break;
        }

        Crop crop = GridPropertiesManager.Instance.GetCropObjectAtGridLocation(gridPropertyDetails);

        if (crop != null)
        {
            switch (equippedItemDetails.itemType)
            {
                case ItemType.Collecting_tool:
                    crop.ProcessToolAction(equippedItemDetails, isPickingRight, isPickingLeft, isPickingDown, isPickingUp);
                    break;
            }
        }
    }

    #region Test Methods
    private void PlayerTestInput()
  {
    if (Input.GetKey(KeyCode.T))
    {
      TimeManager.Instance.TestAdvanceGameMinute();
    }

    if (Input.GetKey(KeyCode.G))
    {
      TimeManager.Instance.TestAdvanceGameDay();
    }
  }
  #endregion

}
