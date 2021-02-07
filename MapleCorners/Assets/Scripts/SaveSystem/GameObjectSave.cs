// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectSave
{
  public Dictionary<string, SceneSave> SceneData;

  public GameObjectSave()
  {
    SceneData = new Dictionary<string, SceneSave>();
  }
  public GameObjectSave(Dictionary<string, SceneSave> sceneData)
  {
    this.SceneData = sceneData;
  }
}
