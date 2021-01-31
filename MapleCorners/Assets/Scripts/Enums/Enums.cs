// "Citation: Unity 2D Game Developer Course Farming RPG"

// List of scenes in the game
public enum SceneName
{
     Scene1_Apartment,
     Scene2_Roof,
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