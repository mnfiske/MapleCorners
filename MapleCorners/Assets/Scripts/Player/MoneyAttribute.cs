using UnityEngine;
using UnityEngine.UI;

public class MoneyAttribute : SingletonMonoBehavior<MoneyAttribute>, ISaveable
{
    public float playerMoney;
    //Serialize will make it visible in the Unity inspector, but it's still private to this script
    [SerializeField] private Text moneyText;

    private string _iSaveableID;
    public string ISaveableID { get { return _iSaveableID; } set { _iSaveableID = value; } }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    // Start is called before the first frame update
    void Start()
    {
        UpdateMoney();
    }

    protected override void Awake()
    {
        base.Awake();

        // Get a guid and store it in the ISaveableID
        ISaveableID = GetComponent<GenerateGuid>().Guid;

        GameObjectSave = new GameObjectSave();
    }

    private void OnEnable()
    {
        ISaveableRegister();
    }

    private void OnDisable()
    {
        ISaveableDeregister();
    }

    public void UpdateMoney()
    {
        // this will update the UI text box that displays energy and set it to the player's energy (converted from float to string)
        moneyText.text = playerMoney.ToString("0.00");
    }

    /// <summary>
    /// Register the money with ISaveable
    /// </summary>
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjects.Add(this);
    }

    /// <summary>
    /// Deregister the money with ISaveable
    /// </summary>
    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjects.Remove(this);
    }

    public GameObjectSave ISaveableSave()
    {
        // Remove existing save data for this scene if it exists
        GameObjectSave.SceneData.Remove(Settings.MainScene);

        SceneSave sceneSave = new SceneSave();

        // Instantiate the dictionary to store the money data
        sceneSave.moneyData = playerMoney;

        // Add the money data to the GameObjectSave
        GameObjectSave.SceneData.Add(Settings.MainScene, sceneSave);

        return GameObjectSave;
    }

    public void ISaveableLoad(GameSave gameSave)
    {
        // Check the save data for the game object
        if (gameSave.gameObjectData.TryGetValue(ISaveableID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;

            // Get the data from the main scene--this is where the energy is stored
            if (GameObjectSave.SceneData.TryGetValue(Settings.MainScene, out SceneSave sceneSave))
            {
                playerMoney = sceneSave.moneyData;
                UpdateMoney();
            }
        }
    }

    public void ISaveableStoreScene(string sceneName)
    {
        // No scene data to store
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        // No scene data to restore
    }
}
