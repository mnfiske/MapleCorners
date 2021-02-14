// "Citation: Unity 2D Game Developer Course Farming RPG"

using UnityEngine;
using System.Collections.Generic;

public class Player : SingletonMonoBehavior<Player>
{

    private AnimationOverrides animationOverrides;
    private GridCursor gridCursor;
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

    private void Start()
    {
      gridCursor = FindObjectOfType<GridCursor>();
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
      if (Input.GetMouseButton(0))
      {
        if (gridCursor.CursorIsEnabled)
        {
          ProcessPlayerClickInput();
        }
      }
    }

    private void ProcessPlayerClickInput()
    {
      ResetMovement();

      ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

      if (itemDetails != null)
      {
        switch (itemDetails.itemType)
        {
          case ItemType.Seed:
            if (Input.GetMouseButtonDown(0))
            {
              ProcessPlayerClickInputSeed(itemDetails);
            }
            break;

          case ItemType.Commodity:
            if (Input.GetMouseButtonDown(0))
            {
              ProcessPlayerClickInputCommodity(itemDetails);
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

    private void ProcessPlayerClickInputSeed(ItemDetails itemDetails)
    {
        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
        {
            EventHandler.CallDropSelectedItemEvent();
        }
    }

    private void ProcessPlayerClickInputCommodity(ItemDetails itemDetails)
    {
        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
        {
            EventHandler.CallDropSelectedItemEvent();
        }
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
