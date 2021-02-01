// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : SingletonMonoBehavior<SaveLoadManager>
{
  public List<ISaveable> iSaveableObjects;

  protected override void Awake()
  {
    base.Awake();

    iSaveableObjects = new List<ISaveable>();
  }

  //public void StoreCurrentSceneData()
  //{
  //  foreach (ISaveable obj in iSaveableObjects)
  //  {
  //    obj.ISavableStoreScene(SceneMans)
  //  }
  //}
}
