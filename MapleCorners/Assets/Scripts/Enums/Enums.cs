﻿// "Citation: Unity 2D Game Developer Course Farming RPG"

// List of scenes in the game
public enum SceneName
{
     Scene1_Apartment,
     Scene2_Roof,
     Scene3_Work,
     Beginning
}

public enum AnimationName
{
    idleDown,
    idleUp,
    idleRight,
    idleLeft,
    walkUp,
    walkDown,
    walkRight,
    walkLeft,
    runUp,
    runDown,
    runRight,
    runLeft,
    useToolUp,
    useToolDown,
    useToolRight,
    useToolLeft,
    swingToolUp,
    swingToolDown,
    swingToolRight,
    swingToolLeft,
    liftToolUp,
    liftToolDown,
    liftToolRight,
    liftToolLeft,
    holdToolUp,
    holdToolDown,
    holdToolRight,
    holdToolLeft,
    pickDown,
    pickUp,
    pickRight,
    pickLeft,
    count
}

public enum CharacterPartAnimator
{
    body,
    arms,
    hair,
    tool,
    hat,
    count
}
public enum PartVariantColour
{
    none,
    count
}

public enum PartVariantType
{
    none,
    carry,
    hoe,
    pickaxe,
    axe,
    scythe,
    wateringCan,
    count
}

public enum InventoryLocation
{
    player,
    chest,
    count
}

public enum ToolEffect
{
    none,
    watering
}

public enum Direction
{
    up,
    down,
    left,
    right,
    none
}
public enum ItemType
{
    Seed,
    Commodity,
    Watering_tool,
    Hoeing_tool,
    Chopping_tool,
    Breaking_tool,
    Reaping_tool,
    Collecting_tool,
    Reapable_scenery,
	Furniture,
    none,
    count
}
/// <summary>
/// The season of the year. Our MVP has only one season, but the season is being included to reduce the refactoring necessary
/// to add seasons if we are able to develop beyond the MVP goals.
/// </summary>
public enum Season
{
  Spring,
  Summer,
  Autumn,
  Winter,
  none,
  count
}

public enum Weekday
{
  Sun,
  Mon,
  Tues,
  Wed,
  Thu,
  Fri,
  Sat
}

public enum GridBoolProperty
{
  diggable,
  canDropItem,
  canPlaceFurniture,
  isPath,               // Stub for future release NPC path finding
  isNPCObstacle,        // Stub for future release NPC path finding
}