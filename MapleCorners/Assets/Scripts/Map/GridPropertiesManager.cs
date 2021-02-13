// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateGuid))]
public class GridPropertiesManager : SingletonMonoBehavior<GridPropertiesManager>, ISaveable
{
  public Grid grid;
  /// <summary>
  /// Dictionary storing the grid property details. The key will be a string holding the coordinate location
  /// </summary>
  private Dictionary<string, GridPropertyDetails> gridPropertyDictionary;
  /// <summary>
  /// An array of the scriptable type objects of type SO_GridProperties
  /// </summary>
  [SerializeField] private SO_GridProperties[] so_gridPropertiesArray = null;

  /// <summary>
  /// Unique ID property
  /// </summary>
  private string _iSaveableUniqueID;
  public string ISaveableID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

  /// <summary>
  /// Object to store the save data for the game object
  /// </summary>
  private GameObjectSave _gameObjectSave;
  public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

  protected override void Awake()
  {
    base.Awake();

    // Generate a unique ID
    ISaveableID = GetComponent<GenerateGuid>().Guid;
    // Create a new instance of GameObjectSave
    GameObjectSave = new GameObjectSave();
  }

  private void OnEnable()
  {
    ISaveableRegister();

    EventHandler.AfterSceneLoadEvent += AfterSceneLoaded;
    //EventHandler.AdvanceGameDayEvent += AdvanceDay;
  }

  private void OnDisable()
  {
    ISaveableDeregister();

    EventHandler.AfterSceneLoadEvent -= AfterSceneLoaded;
    //EventHandler.AdvanceGameDayEvent -= AdvanceDay;
  }

  public void ISaveableDeregister()
  {
    SaveLoadManager.Instance.iSaveableObjects.Remove(this);
  }

  public void ISaveableRegister()
  {
    SaveLoadManager.Instance.iSaveableObjects.Add(this);
  }

  private void AfterSceneLoaded()
  {
    // Populate the grid member variable
    grid = GameObject.FindObjectOfType<Grid>();
  }

  private void Start()
  {
    InitializeGridProperties();
  }

  /// <summary>
  /// 
  /// </summary>
  private void InitializeGridProperties()
  {
    // Loop through all the scriptable object assets which store the grid property values 
    foreach (SO_GridProperties so_GridProperties in so_gridPropertiesArray)
    {
      // Create a dictionary to store the grid properties
      Dictionary<string, GridPropertyDetails> gridPropertyDictionary = new Dictionary<string, GridPropertyDetails>();

      // Loop through the GridPropertyList & pull out the grid property from the scriptable object 
      foreach (GridProperty gridProperty in so_GridProperties.GridPropertyList)
      {
        GridPropertyDetails gridPropertyDetails;

        // Populate the gridPropertyDetails from the grid property from the scriptable object asset
        gridPropertyDetails = GetGridPropertyDetails(gridProperty.GridCoordinate.X, gridProperty.GridCoordinate.Y, gridPropertyDictionary);

        // If gridPropertyDetails is null, it didn't already exist in our dictionary, so create a new one
        if (gridPropertyDetails == null)
        {
          gridPropertyDetails = new GridPropertyDetails();
        }

        switch (gridProperty.GridBoolProperty)
        {
          case GridBoolProperty.diggable:
            gridPropertyDetails.IsDiggable = gridProperty.GridBoolValue;
            break;

          case GridBoolProperty.canDropItem:
            gridPropertyDetails.CanDropItem = gridProperty.GridBoolValue;
            break;

          case GridBoolProperty.canPlaceFurniture:
            gridPropertyDetails.CanPlaceFurniture = gridProperty.GridBoolValue;
            break;

          default:
            break;
        }

        SetGridPropertyDetails(gridProperty.GridCoordinate.X, gridProperty.GridCoordinate.Y, gridPropertyDetails, gridPropertyDictionary);
      }

      SceneSave sceneSave = new SceneSave();

      // Store the gridPropertyDictionary in the SceneSave's GridPropertyDetailsDictionary 
      sceneSave.GridPropertyDetailsDictionary = gridPropertyDictionary;

      // If the current scene is the starting scene, set the gridPropertyDictionary member variable to the currently populated gridPropertyDictionary
      if (so_GridProperties.SceneName.ToString() == SceneControllerManager.Instance.startingSceneName.ToString())
      {
        this.gridPropertyDictionary = gridPropertyDictionary;
      }

      // Add the SceneSave with the populated GridPropertyDetailsDictionary to the GameObjectSave SceneData
      GameObjectSave.SceneData.Add(so_GridProperties.SceneName.ToString(), sceneSave);
    }
  }

  /// <summary>
  /// Find grid properties stored in the passed-in dictionary based on the grid coordinates
  /// </summary>
  /// <param name="gridX"></param>
  /// <param name="gridY"></param>
  /// <returns></returns>
  public GridPropertyDetails GetGridPropertyDetails(int gridX, int gridY)
  {
    return GetGridPropertyDetails(gridX, gridY, gridPropertyDictionary);
  }

  /// <summary>
  /// Find grid properties stored in the current gridPropertyDictionary based on the grid coordinates
  /// </summary>
  /// <param name="gridX"></param>
  /// <param name="gridY"></param>
  /// <param name="gridPropertyDictionary"></param>
  /// <returns></returns>
  public GridPropertyDetails GetGridPropertyDetails(int gridX, int gridY, Dictionary<string, GridPropertyDetails> gridPropertyDictionary)
  {
    // Build a string based on the coordinates to be used as the dictionary key
    string key = "x" + gridX + "y" + gridY;

    GridPropertyDetails gridPropertyDetails;

    // Look for the key in the dictionary
    if (gridPropertyDictionary.TryGetValue(key, out gridPropertyDetails))
    {
      return gridPropertyDetails;
    }
    else
    {
      // The key wasn't stored in the dictionary, return null
      return null;
    }
  }

  /// <summary>
  /// Store grid properties in the current gridPropertyDictionary, base the key on the grid coordinates
  /// </summary>
  /// <param name="gridX"></param>
  /// <param name="gridY"></param>
  /// <param name="gridPropertyDetails"></param>
  public void SetGridPropertyDetails(int gridX, int gridY, GridPropertyDetails gridPropertyDetails)
  {
    SetGridPropertyDetails(gridX, gridY, gridPropertyDetails, gridPropertyDictionary);
  }

  /// <summary>
  /// Store grid properties in the passed-in dictionary, base the key on the grid coordinates
  /// </summary>
  /// <param name="gridX"></param>
  /// <param name="gridY"></param>
  /// <param name="gridPropertyDetails"></param>
  /// <param name="gridPropertyDictionary"></param>
  public void SetGridPropertyDetails(int gridX, int gridY, GridPropertyDetails gridPropertyDetails, Dictionary<string, GridPropertyDetails> gridPropertyDictionary)
  {
    // Build a string based on the coordinates to be used as the dictionary key
    string key = "x" + gridX + "y" + gridY;

    gridPropertyDetails.GridX = gridX;
    gridPropertyDetails.GridY = gridY;

    // Store the gridPropertyDetails in the dictionary
    gridPropertyDictionary[key] = gridPropertyDetails;
  }

  /// <summary>
  /// Saves the GridPropertyDetailsDictionary for the current scene
  /// </summary>
  /// <param name="sceneName"></param>
  public void ISaveableStoreScene(string sceneName)
  {
    // Remove any old SceneData stored for the scene
    GameObjectSave.SceneData.Remove(sceneName);

    SceneSave sceneSave = new SceneSave();

    // Store the gridPropertyDictionary in the SceneSave
    sceneSave.GridPropertyDetailsDictionary = gridPropertyDictionary;

    // Add the SceneData to the GameObjectSave
    GameObjectSave.SceneData.Add(sceneName, sceneSave);
  }

  /// <summary>
  /// Restores the scene data
  /// </summary>
  /// <param name="sceneName"></param>
  public void ISaveableRestoreScene(string sceneName)
  {
    // Check if SceneData already exists for the scene
    if (GameObjectSave.SceneData.TryGetValue(sceneName, out SceneSave sceneSave))
    {
      // If so, check if that SceneData has a GridPropertyDetailsDictionary
      if (sceneSave.GridPropertyDetailsDictionary != null)
      {
        // If so, set the gridPropertyDictionary to the stored dictionary
        gridPropertyDictionary = sceneSave.GridPropertyDetailsDictionary;
      }
    }
  }
}
