// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCoordinate
{
  // X & Y grid coordinates on the tilemap
  public int X;
  public int Y;

  public GridCoordinate(int xCoordinate, int yCoordinate)
  {
    X = xCoordinate;
    Y = yCoordinate;
  }

  // Explicit type conversion for converting GridCoordinate to Vector2
  public static explicit operator Vector2(GridCoordinate gridCoordinate)
  {
    return new Vector2((float)gridCoordinate.X, (float)gridCoordinate.Y);
  }

  // Explicit type conversion for converting GridCoordinate to Vector2Int
  public static explicit operator Vector2Int(GridCoordinate gridCoordinate)
  {
    return new Vector2Int(gridCoordinate.X, gridCoordinate.Y);
  }

  // Explicit type conversion for converting GridCoordinate to Vector3
  public static explicit operator Vector3(GridCoordinate gridCoordinate)
  {
    return new Vector3((float)gridCoordinate.X, (float)gridCoordinate.Y, 0f);
  }

  // Explicit type conversion for converting GridCoordinate to Vector3Int
  public static explicit operator Vector3Int(GridCoordinate gridCoordinate)
  {
    return new Vector3Int(gridCoordinate.X, gridCoordinate.Y, 0);
  }
}
