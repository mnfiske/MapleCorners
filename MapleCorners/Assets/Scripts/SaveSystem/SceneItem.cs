// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneItem
{
  public int itemID;
  public string itemName;
  public Vector3Serializable location;

  public SceneItem()
  {
    location = new Vector3Serializable();
  }
}
