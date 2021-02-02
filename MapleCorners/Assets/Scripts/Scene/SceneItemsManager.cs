// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(GenerateGuid))]
//public class SceneItemsManager : SingletonMonoBehavior<SceneItemsManager>, ISaveable
//{
//  private Transform parentItem;
//  [SerializeField] private GameObject itemPrefab = null;

//  private string _iSaveableID;
//  public string ISaveableID
//  {
//    get { return _iSaveableID; }
//    set { _iSaveableID = value; }
//  }

//  private GameObjectSave _gameObjectSave;
//  public GameObjectSave GameObjectSave
//  {
//    get { return _gameObjectSave; }
//    set { _gameObjectSave = value; }
//  }

//  private void AfterSceneLoad()
//  {
//    parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
//  }

//  protected override void Awake()
//  {
//    base.Awake();

//    ISaveableID = GetComponent<GenerateGuid>().Guid;
//    GameObjectSave = new GameObjectSave();
//  }

//  private void DestroySceneItems()
//  {
    
//  }
//}
