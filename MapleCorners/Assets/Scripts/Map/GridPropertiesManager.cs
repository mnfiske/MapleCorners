// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GenerateGuid))]
public class GridPropertiesManager : SingletonMonoBehavior<GridPropertiesManager>, ISaveable
{
    private Tilemap floorDecoration1;
    private Tilemap floorDecoration2;

    private Transform cropParentTransform;

    private Grid grid;
    /// <summary>
    /// Dictionary storing the grid property details. The key will be a string holding the coordinate location
    /// </summary>
    private Dictionary<string, GridPropertyDetails> gridPropertyDictionary;
    /// <summary>
    /// An array of the scriptable type objects of type SO_GridProperties
    /// </summary>
    [SerializeField] private SO_GridProperties[] so_gridPropertiesArray = null;
    [SerializeField] private Tile[] dugGround = null;
    [SerializeField] private Tile[] wateredGround = null;

    [SerializeField] private SO_CropDetailsList so_CropDetailsList = null;

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
        EventHandler.AdvanceGameDayEvent += AdvanceDay;
    }

    private void OnDisable()
    {
        ISaveableDeregister();

        EventHandler.AfterSceneLoadEvent -= AfterSceneLoaded;
        EventHandler.AdvanceGameDayEvent -= AdvanceDay;
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
        if (GameObject.FindGameObjectWithTag(Tags.CropsParentTransform) != null)
        {
            cropParentTransform = GameObject.FindGameObjectWithTag(Tags.CropsParentTransform).transform;
        }
        else
        {
            cropParentTransform = null;
        }

        // Populate the grid member variable
        grid = GameObject.FindObjectOfType<Grid>();

        floorDecoration1 = GameObject.FindGameObjectWithTag(Tags.FloorDecoration1).GetComponent<Tilemap>();
        floorDecoration2 = GameObject.FindGameObjectWithTag(Tags.FloorDecoration2).GetComponent<Tilemap>();
    }

    private void Start()
    {
        InitializeGridProperties();
    }

    /// <summary>
    /// Clear the floor decorations
    /// </summary>
    private void ClearDisplayFloorDecorations()
    {
        // Remove ground decorations
        floorDecoration1.ClearAllTiles();
        floorDecoration2.ClearAllTiles();
    }

    /// <summary>
    /// Clears the floor decorations and planted crops
    /// </summary>
    private void ClearDisplayGridPropertyDetails()
    {
        ClearDisplayFloorDecorations();

        ClearDisplayAllPlantedCrops();
    }

    /// <summary>
    /// Destroys all the crops in the scene
    /// </summary>
    private void ClearDisplayAllPlantedCrops()
    {
        Crop[] crops;
        crops = FindObjectsOfType<Crop>();

        foreach (Crop crop in crops)
        {
            Destroy(crop.gameObject);
        }
    }

    /// <summary>
    /// If the ground at the grid location has been dug, call ConnectDugGround
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    public void DisplayDugGround(GridPropertyDetails gridPropertyDetails)
    {
        // If the ground has been dug, call ConnectDugGround
        if (gridPropertyDetails.DaysSinceDug > -1)
        {
            ConnectDugGround(gridPropertyDetails);
        }
    }

    /// <summary>
    /// Set the tiles to make dug ground appear to be connected with dug ground surrounding it
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    private void ConnectDugGround(GridPropertyDetails gridPropertyDetails)
    {
        // Determine which tile should be set based on the surrounding tiles
        Tile dugTile0 = SetDugTile(gridPropertyDetails.GridX, gridPropertyDetails.GridY);
        // Set the tile
        floorDecoration1.SetTile(new Vector3Int(gridPropertyDetails.GridX, gridPropertyDetails.GridY, 0), dugTile0);

        GridPropertyDetails adjacentGridPropertyDetails;
        // Check the tile above the current tile--is it also dug ground?
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.GridX, gridPropertyDetails.GridY + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.DaysSinceDug > -1)
        {
            // If so, we need to determine which tile should be set based on the surrounding tiles
            Tile dugTile1 = SetDugTile(gridPropertyDetails.GridX, gridPropertyDetails.GridY + 1);
            // Set the tile
            floorDecoration1.SetTile(new Vector3Int(gridPropertyDetails.GridX, gridPropertyDetails.GridY + 1, 0), dugTile1);
        }
        // Check the tile below the current tile--is it also dug ground?
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.GridX, gridPropertyDetails.GridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.DaysSinceDug > -1)
        {
            // If so, we need to determine which tile should be set based on the surrounding tiles
            Tile dugTile2 = SetDugTile(gridPropertyDetails.GridX, gridPropertyDetails.GridY - 1);
            // Set the tile
            floorDecoration1.SetTile(new Vector3Int(gridPropertyDetails.GridX, gridPropertyDetails.GridY - 1, 0), dugTile2);
        }
        // Check the tile to the left of the current tile--is it also dug ground?
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.GridX - 1, gridPropertyDetails.GridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.DaysSinceDug > -1)
        {
            // If so, we need to determine which tile should be set based on the surrounding tiles
            Tile dugTile3 = SetDugTile(gridPropertyDetails.GridX - 1, gridPropertyDetails.GridY);
            // Set the tile
            floorDecoration1.SetTile(new Vector3Int(gridPropertyDetails.GridX - 1, gridPropertyDetails.GridY, 0), dugTile3);
        }
        // Check the tile to the right of the current tile--is it also dug ground?
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.GridX + 1, gridPropertyDetails.GridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.DaysSinceDug > -1)
        {
            // If so, we need to determine which tile should be set based on the surrounding tiles
            Tile dugTile4 = SetDugTile(gridPropertyDetails.GridX + 1, gridPropertyDetails.GridY);
            // Set the tile
            floorDecoration1.SetTile(new Vector3Int(gridPropertyDetails.GridX + 1, gridPropertyDetails.GridY, 0), dugTile4);
        }
    }

    /// <summary>
    /// Takes the X and Y coordinates & returns the tile that should be used based on the surrounding tiles
    /// </summary>
    private Tile SetDugTile(int xGrid, int yGrid)
    {
        // Store whether the squares adjacent to the passed in coordinates have been dug
        bool upDug = IsGridSquareDug(xGrid, yGrid + 1);
        bool downDug = IsGridSquareDug(xGrid, yGrid - 1);
        bool leftDug = IsGridSquareDug(xGrid - 1, yGrid);
        bool rightDug = IsGridSquareDug(xGrid + 1, yGrid);

        // Return which tile should be applied to the grid square based on the state of the surrounding tiles
        if (!upDug && !downDug && !rightDug && !leftDug)
        {
            return dugGround[0];
        }
        else if (!upDug && downDug && rightDug && !leftDug)
        {
            return dugGround[1];
        }
        else if (!upDug && downDug && rightDug && leftDug)
        {
            return dugGround[2];
        }
        else if (!upDug && downDug && !rightDug && leftDug)
        {
            return dugGround[3];
        }
        else if (!upDug && downDug && !rightDug && !leftDug)
        {
            return dugGround[4];
        }
        else if (upDug && downDug && rightDug && !leftDug)
        {
            return dugGround[5];
        }
        else if (upDug && downDug && rightDug && leftDug)
        {
            return dugGround[6];
        }
        else if (upDug && downDug && !rightDug && leftDug)
        {
            return dugGround[7];
        }
        else if (upDug && downDug && !rightDug && !leftDug)
        {
            return dugGround[8];
        }
        else if (upDug && !downDug && rightDug && !leftDug)
        {
            return dugGround[9];
        }
        else if (upDug && !downDug && rightDug && leftDug)
        {
            return dugGround[10];
        }
        else if (upDug && !downDug && !rightDug && leftDug)
        {
            return dugGround[11];
        }
        else if (upDug && !downDug && !rightDug && !leftDug)
        {
            return dugGround[12];
        }
        else if (!upDug && !downDug && rightDug && !leftDug)
        {
            return dugGround[13];
        }
        else if (!upDug && !downDug && rightDug && leftDug)
        {
            return dugGround[14];
        }
        else if (!upDug && !downDug && !rightDug && leftDug)
        {
            return dugGround[15];
        }

        return null;
    }

    /// <summary>
    /// Returns whether the grid at the passed in coordinates is dug ground
    /// </summary>
    /// <param name="xGrid"></param>
    /// <param name="yGrid"></param>
    /// <returns></returns>
    private bool IsGridSquareDug(int xGrid, int yGrid)
    {
        GridPropertyDetails gridPropertyDetails = GetGridPropertyDetails(xGrid, yGrid);

        if (gridPropertyDetails == null)
        {
            return false;
        }
        else if (gridPropertyDetails.DaysSinceDug > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// If the ground has been watered, call ConnectWateredGround
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    public void DisplayWateredGround(GridPropertyDetails gridPropertyDetails)
    {
        if (gridPropertyDetails.DaysSinceWatered > -1)
        {
            ConnectWateredGround(gridPropertyDetails);
        }
    }

    /// <summary>
    /// Set the tiles to make watered ground appear to be connected with watered ground surrounding it
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    private void ConnectWateredGround(GridPropertyDetails gridPropertyDetails)
    {
        // Determine which tile should be set based on the surrounding tiles
        Tile wateredTile0 = SetWateredTile(gridPropertyDetails.GridX, gridPropertyDetails.GridY);
        //Set the tile
        floorDecoration2.SetTile(new Vector3Int(gridPropertyDetails.GridX, gridPropertyDetails.GridY, 0), wateredTile0);

        GridPropertyDetails adjacentGridPropertyDetails;

        // Check the tile above the current tile--is it also watered ground?
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.GridX, gridPropertyDetails.GridY + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.DaysSinceWatered > -1)
        {
            // If so, we need to determine which tile should be set based on the surrounding tiles
            Tile wateredTile1 = SetWateredTile(gridPropertyDetails.GridX, gridPropertyDetails.GridY + 1);
            // Set the tile
            floorDecoration2.SetTile(new Vector3Int(gridPropertyDetails.GridX, gridPropertyDetails.GridY + 1, 0), wateredTile1);
        }
        // Check the tile below the current tile--is it also watered ground?
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.GridX, gridPropertyDetails.GridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.DaysSinceWatered > -1)
        {
            // If so, we need to determine which tile should be set based on the surrounding tiles
            Tile wateredTile2 = SetWateredTile(gridPropertyDetails.GridX, gridPropertyDetails.GridY - 1);
            // Set the tile
            floorDecoration2.SetTile(new Vector3Int(gridPropertyDetails.GridX, gridPropertyDetails.GridY - 1, 0), wateredTile2);
        }
        // Check the tile to the left of the current tile--is it also watered ground?
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.GridX - 1, gridPropertyDetails.GridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.DaysSinceWatered > -1)
        {
            // If so, we need to determine which tile should be set based on the surrounding tiles
            Tile wateredTile3 = SetWateredTile(gridPropertyDetails.GridX - 1, gridPropertyDetails.GridY);
            // Set the tile
            floorDecoration2.SetTile(new Vector3Int(gridPropertyDetails.GridX - 1, gridPropertyDetails.GridY, 0), wateredTile3);
        }
        // Check the tile to the right of the current tile--is it also watered ground?
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.GridX + 1, gridPropertyDetails.GridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.DaysSinceWatered > -1)
        {
            // If so, we need to determine which tile should be set based on the surrounding tiles
            Tile wateredTile4 = SetWateredTile(gridPropertyDetails.GridX + 1, gridPropertyDetails.GridY);
            // Set the tile
            floorDecoration2.SetTile(new Vector3Int(gridPropertyDetails.GridX + 1, gridPropertyDetails.GridY, 0), wateredTile4);
        }
    }

    /// <summary>
    /// Returns the watered ground tile that should be set based on the state of the surrounding tiles
    /// </summary>
    /// <param name="xGrid"></param>
    /// <param name="yGrid"></param>
    /// <returns></returns>
    private Tile SetWateredTile(int xGrid, int yGrid)
    {
        // Store whether the squares adjacent to the passed in coordinates have been watered
        bool upWatered = IsGridSquareWatered(xGrid, yGrid + 1);
        bool downWatered = IsGridSquareWatered(xGrid, yGrid - 1);
        bool leftWatered = IsGridSquareWatered(xGrid - 1, yGrid);
        bool rightWatered = IsGridSquareWatered(xGrid + 1, yGrid);

        // Return which tile should be applied to the grid square based on the state of the surrounding tiles
        if (!upWatered && !downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[0];
        }
        else if (!upWatered && downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[1];
        }
        else if (!upWatered && downWatered && rightWatered && leftWatered)
        {
            return wateredGround[2];
        }
        else if (!upWatered && downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[3];
        }
        else if (!upWatered && downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[4];
        }
        else if (upWatered && downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[5];
        }
        else if (upWatered && downWatered && rightWatered && leftWatered)
        {
            return wateredGround[6];
        }
        else if (upWatered && downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[7];
        }
        else if (upWatered && downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[8];
        }
        else if (upWatered && !downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[9];
        }
        else if (upWatered && !downWatered && rightWatered && leftWatered)
        {
            return wateredGround[10];
        }
        else if (upWatered && !downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[11];
        }
        else if (upWatered && !downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[12];
        }
        else if (!upWatered && !downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[13];
        }
        else if (!upWatered && !downWatered && rightWatered && leftWatered)
        {
            return wateredGround[14];
        }
        else if (!upWatered && !downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[15];
        }

        return null;
    }

    /// <summary>
    /// Return whether the frid square at the given coordinates has been watered
    /// </summary>
    /// <param name="xGrid"></param>
    /// <param name="yGrid"></param>
    /// <returns></returns>
    private bool IsGridSquareWatered(int xGrid, int yGrid)
    {
        GridPropertyDetails gridPropertyDetails = GetGridPropertyDetails(xGrid, yGrid);

        if (gridPropertyDetails == null)
        {
            return false;
        }
        else if (gridPropertyDetails.DaysSinceWatered > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Goes through every item in gridPropertyDictionary & calls the display methods
    /// </summary>
    private void DisplayGridPropertyDetails()
    {
        foreach (KeyValuePair<string, GridPropertyDetails> gridProperty in gridPropertyDictionary)
        {
            GridPropertyDetails gridPropertyDetails = gridProperty.Value;

            DisplayDugGround(gridPropertyDetails);

            DisplayWateredGround(gridPropertyDetails);

            DisplayPlantedCrop(gridPropertyDetails);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    public void DisplayPlantedCrop(GridPropertyDetails gridPropertyDetails)
    {
        if (gridPropertyDetails.SeedItemCode > -1)
        {
            CropDetails cropDetails = so_CropDetailsList.GetCropDetails(gridPropertyDetails.SeedItemCode);

            if (cropDetails != null)
            {
                GameObject cropPrefab;

                int growthStages = cropDetails.growthDays.Length;
                int daysCounter = cropDetails.totalGrowthDays;
                int currentGrowthStage = 0;

                for (int i = growthStages - 1; i >= 0; i--)
                {
                    if (gridPropertyDetails.GrowthDays >= daysCounter)
                    {
                        currentGrowthStage = i;
                        break;
                    }

                    daysCounter = daysCounter - cropDetails.growthDays[i];
                }

                cropPrefab = cropDetails.growthPrefab[currentGrowthStage];

                Sprite growthSprite = cropDetails.growthSprite[currentGrowthStage];

                Vector3 worldPosition = floorDecoration2.CellToWorld(new Vector3Int(gridPropertyDetails.GridX, gridPropertyDetails.GridY, 0));

                worldPosition = new Vector3(worldPosition.x + Settings.gridCellSize / 2, worldPosition.y, worldPosition.z);

                GameObject cropInstance = Instantiate(cropPrefab, worldPosition, Quaternion.identity);

                cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = growthSprite;
                cropInstance.transform.SetParent(cropParentTransform);
                cropInstance.GetComponent<Crop>().cropGridPosition = new Vector2Int(gridPropertyDetails.GridX, gridPropertyDetails.GridY);
            }
        }
    }

    /// <summary>
    /// Set up the grid properties with their values from the map
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

            if (gridPropertyDictionary.Count > 0)
            {
                // Clear the existing displays
                ClearDisplayGridPropertyDetails();

                // Display based on grid properties of current scene
                DisplayGridPropertyDetails();
            }
        }
    }

    /// <summary>
    /// Restores the data for the current scene
    /// </summary>
    /// <param name="gameSave"></param>
    public void ISaveableLoad(GameSave gameSave)
    {
        // Try to get the value with the key of the ISaveableID
        if (gameSave.gameObjectData.TryGetValue(ISaveableID, out GameObjectSave gameObjectSave))
        {
            // If we got an object, store it in GameObjectSave
            GameObjectSave = gameObjectSave;

            // For the current scene, restore the data
            ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// Generate the save data
    /// </summary>
    /// <returns></returns>
    public GameObjectSave ISaveableSave()
    {
        // Store the data for the current scene
        ISaveableStoreScene(SceneManager.GetActiveScene().name);

        return GameObjectSave;
    }

    /// <summary>
    /// Change any of the grid property details that needs to respond to a change in day (all scenes)
    /// </summary>
    /// <param name="gameYear"></param>
    /// <param name="gameSeason"></param>
    /// <param name="gameDay"></param>
    /// <param name="gameDayOfWeek"></param>
    /// <param name="gameHour"></param>
    /// <param name="gameMinute"></param>
    /// <param name="gameSecond"></param>
    private void AdvanceDay(int gameYear, Season gameSeason, int gameDay, Weekday gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        // Clear the display grid property details for dug and watered ground
        ClearDisplayGridPropertyDetails();

        // Loop through all grid property details in all scenes
        foreach (SO_GridProperties so_GridProperties in so_gridPropertiesArray)
        {
            if (GameObjectSave.SceneData.TryGetValue(so_GridProperties.SceneName.ToString(), out SceneSave sceneSave))
            {
                if (sceneSave.GridPropertyDetailsDictionary != null)
                {
                    for (int i = sceneSave.GridPropertyDetailsDictionary.Count - 1; i >= 0; i--)
                    {
                        KeyValuePair<string, GridPropertyDetails> details = sceneSave.GridPropertyDetailsDictionary.ElementAt(i);

                        GridPropertyDetails gridPropertyDetails = details.Value;

                        // Grow any planted crop and reset days since watered to -1
                        if (gridPropertyDetails.DaysSinceWatered > -1)
                        {
                            // Update the GrowthDays if a crop is planted here
                            if (gridPropertyDetails.GrowthDays > -1)
                            {
                                gridPropertyDetails.GrowthDays += 1;
                            }
                            gridPropertyDetails.DaysSinceWatered = -1;
                        }

                        // Update the grid property details
                        SetGridPropertyDetails(gridPropertyDetails.GridX, gridPropertyDetails.GridY, gridPropertyDetails, sceneSave.GridPropertyDetailsDictionary);
                    }
                }
            }
        }

        // Update the display tiles
        DisplayGridPropertyDetails();
    }

    /// <summary>
    /// Returns the crop component if there is a crop growing at the passed in GridPropertyDetails
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <returns></returns>
    public Crop GetCropObjectAtGridLocation(GridPropertyDetails gridPropertyDetails)
    {
        // Get the world position of the gridPropertyDetails
        Vector3 worldPosition = grid.GetCellCenterWorld(new Vector3Int(gridPropertyDetails.GridX, gridPropertyDetails.GridY, 0));
        // Get the colliders overlapping the world position
        Collider2D[] collider2DArray = Physics2D.OverlapPointAll(worldPosition);

        // Check if we have a crop at this point
        Crop crop = null;
        for (int i = 0; i < collider2DArray.Length; i++)
        {
            crop = collider2DArray[i].gameObject.GetComponentInParent<Crop>();
            if (crop != null && crop.cropGridPosition == new Vector2Int(gridPropertyDetails.GridX, gridPropertyDetails.GridY))
                break;
            crop = collider2DArray[i].gameObject.GetComponentInChildren<Crop>();
            if (crop != null && crop.cropGridPosition == new Vector2Int(gridPropertyDetails.GridX, gridPropertyDetails.GridY))
                break;
        }

        return crop;
    }

    /// <summary>
    /// Returns the cropDetails for the passed in seedItemCode
    /// </summary>
    /// <param name="seedItemCode"></param>
    /// <returns></returns>
    public CropDetails GetCropDetails(int seedItemCode)
    {
        return so_CropDetailsList.GetCropDetails(seedItemCode);
    }
}
