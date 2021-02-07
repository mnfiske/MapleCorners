// "Citation: Unity 2D Game Developer Course Farming RPG"

// List of scenes in the game
public enum SceneName
{
     Scene1_Apartment,
     Scene2_Roof
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

