// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneSave
{
  public List<SceneItem> ListSceneItem;
  public Dictionary<string, GridPropertyDetails> GridPropertyDetailsDictionary;
}
