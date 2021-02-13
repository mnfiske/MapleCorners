// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridPropertyDetails
{
  // Grid location properties
  public int GridX;
  public int GridY;
  // Properties to hold the grid properties
  public bool IsDiggable = false;
  public bool CanDropItem = false;
  public bool CanPlaceFurniture = false;
  public bool IsPath = false;
  public bool IsNPCObstacle = false;
  // Crop properties
  public int DaysSinceDug = -1;
  public int DaysSinceWatered = -1;
  public int SeedItemCode = -1;
  public int GrowthDays = -1;
  public int DaysSinceLastHarvest = -1;

  public GridPropertyDetails()
  {
  }
}
