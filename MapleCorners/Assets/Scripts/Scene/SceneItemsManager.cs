// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateGuid))]
public class SceneItemsManager : SingletonMonoBehavior<SceneItemsManager>, ISaveable
{
  /// <summary>
  /// The parent item game object in the scene
  /// </summary>
  private Transform parentItem;
  /// <summary>
  /// The item prefab
  /// </summary>
  [SerializeField] private GameObject itemPrefab = null;

  /// <summary>
  /// The unique ID
  /// </summary>
  private string _iSaveableID;
  public string ISaveableID
  {
    get { return _iSaveableID; }
    set { _iSaveableID = value; }
  }

  /// <summary>
  /// Object to store the save data & scene save data for the game object
  /// </summary>
  private GameObjectSave _gameObjectSave;
  public GameObjectSave GameObjectSave
  {
    get { return _gameObjectSave; }
    set { _gameObjectSave = value; }
  }

  protected override void Awake()
  {
    base.Awake();

    // Populate the ISaveableID w/ the GUID value from the GenerateGuid component
    ISaveableID = GetComponent<GenerateGuid>().Guid;
    GameObjectSave = new GameObjectSave();
  }

  /// <summary>
  /// Sets the parentItem
  /// </summary>
  private void AfterSceneLoad()
  {
    parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
  }

  private void DestroySceneItems()
  {
    Item[] itemsInScene = GameObject.FindObjectsOfType<Item>();

    foreach (Item item in itemsInScene)
    {
      Destroy(item.gameObject);
    }
  }

  private void OnEnable()
  {
    // Add the game object to the iSaveableObjects list
    ISaveableRegister();
    // Subscribe to AfterSceneLoadEvent
    EventHandler.AfterLoadSceneEvent += AfterSceneLoad;
  }

  private void OnDisable()
  {
    // Remove the game object from the iSaveableObjects list
    ISaveableDeregister();
    // Unscribe from AfterSceneLoadEvent
    EventHandler.AfterLoadSceneEvent += AfterSceneLoad;
  }

  /// <summary>
  /// Instantiates a single scene item
  /// </summary>
  /// <param name="itemID"></param>
  /// <param name="itemLocation"></param>
  public void InstantiateSceneItem(int itemID, Vector3 itemLocation)
  {
    GameObject itemObj = Instantiate(itemPrefab, itemLocation, Quaternion.identity, parentItem);
    Item item = itemObj.GetComponent<Item>();
    item.Init(itemID);
  }

  /// <summary>
  /// For each item in the sceneItems list, instantiates a game object
  /// </summary>
  /// <param name="sceneItems"></param>
  private void instantiateSceneItems(List<SceneItem> sceneItems)
  {
    GameObject itemObj;

    foreach( SceneItem sceneItem in sceneItems)
    {
      itemObj = Instantiate(itemPrefab, new Vector3(sceneItem.location.x, sceneItem.location.y, sceneItem.location.z), Quaternion.identity, parentItem);

      Item item = itemObj.GetComponent<Item>();
      item.ItemCode = sceneItem.itemCode;
      item.name = sceneItem.itemName;
    }
  }

  /// <summary>
  /// Add the game object to the iSaveableObjects list
  /// </summary>
  public void ISaveableRegister()
  {
    SaveLoadManager.Instance.iSaveableObjects.Add(this);
  }

  /// <summary>
  /// Remove the game object from the iSaveableObjects list
  /// </summary>
  public void ISaveableDeregister()
  {
    SaveLoadManager.Instance.iSaveableObjects.Remove(this);
  }

  /// <summary>
  /// Store items in the scene
  /// </summary>
  /// <param name="name"></param>
  public void ISaveableStoreScene(string name)
  {
    // If we already have an entry for this scene's name in the SceneData dictionary, remove it as we're going to be replacing it.
    GameObjectSave.SceneData.Remove(name);

    // Create a list of all items in the scene
    List<SceneItem> sceneItems = new List<SceneItem>();
    Item[] itemsInScene = FindObjectsOfType<Item>();

    // Populate the list
    foreach (Item item in itemsInScene)
    {
      SceneItem sceneItem = new SceneItem();
      sceneItem.itemCode = item.ItemCode;
      sceneItem.location = new Vector3Serializable(item.transform.position.x, item.transform.position.y, item.transform.position.z);
      sceneItem.itemName = item.name;

      sceneItems.Add(sceneItem);
    }

    // Save our scene item list to our sceneItemDict dictionary
    SceneSave sceneSave = new SceneSave();
    sceneSave.sceneItemDict = new Dictionary<string, List<SceneItem>>();
    sceneSave.sceneItemDict.Add("sceneItems", sceneItems);

    // Add our data to the SceneData dictionary
    GameObjectSave.SceneData.Add(name, sceneSave);
  }

  /// <summary>
  /// Restore any items which were previously stored for the scene
  /// </summary>
  /// <param name="name"></param>
  public void ISaveableRestoreScene(string name)
  {
    // Check if we have a dictionary item for this scene
    if (GameObjectSave.SceneData.TryGetValue(name, out SceneSave sceneSave))
    {
      // We found a dictionary item for this scene

      // Checks if the found dictionary items contains a sceneItemDict field & if that field exists tries to get the sceneItems list
      if (sceneSave.sceneItemDict != null && sceneSave.sceneItemDict.TryGetValue("sceneItems", out List<SceneItem> sceneItems))
      {
        // We got the sceneItems list

        // Destory all the items currently in the scene
        DestroySceneItems();

        // Instantiate all the items in the sceneItems list
        instantiateSceneItems(sceneItems);
      }
    }
  }
}
