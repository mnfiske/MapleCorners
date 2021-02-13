// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridProperty
{
  public GridCoordinate GridCoordinate;
  public GridBoolProperty GridBoolProperty;
  public bool GridBoolValue = false;

  public GridProperty(GridCoordinate gridCoordinate, GridBoolProperty gridBoolProperty, bool gridBoolValue)
  {
    this.GridCoordinate = gridCoordinate;
    this.GridBoolProperty = gridBoolProperty;
    this.GridBoolValue = gridBoolValue;
  }
}
