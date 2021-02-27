// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Singleton 
/// </summary>
public class SaveLoadManager : SingletonMonoBehavior<SaveLoadManager>
{
    /// <summary>
    /// Object to store save data
    /// </summary>
    public GameSave gameSave;

    /// <summary>
    /// Holds all the objects in the scene which implement the ISaveable interface
    /// All objects which implement the ISaveable interface should add themselves to
    /// this list on Awaken
    /// </summary>
    public List<ISaveable> iSaveableObjects;

    /// <summary>
    /// On Awake, calls the base Awake, then creates the list to hold ISaveable objects
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        iSaveableObjects = new List<ISaveable>();
    }

    /// <summary>
    /// Loops thru all objects in the iSaveableObjects list & calls the applicable ISaveableStoreScene
    /// for the object & passes in the name ofthe current scene
    /// </summary>
    public void StoreCurrentSceneData()
    {
        foreach (ISaveable obj in iSaveableObjects)
        {
            obj.ISaveableStoreScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// Loops thru all objects in the iSaveableObjects list & calls the applicable ISaveableRestoreScene
    /// for the object 
    /// </summary>
    public void RestoreCurrentSceneData()
    {
        foreach (ISaveable obj in iSaveableObjects)
        {
            obj.ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// Load the saved data
    /// </summary>
    public void LoadDataFromFile()
    {
        BinaryFormatter bf = new BinaryFormatter();

        // Check if we have a file we can load
        if (File.Exists(Application.persistentDataPath + "/MapleCorners.dat"))
        {
            // Create a new GameSave object
            gameSave = new GameSave();

            // Open the file stream
            FileStream file = File.Open(Application.persistentDataPath + "/MapleCorners.dat", FileMode.Open);

            // Deserialize the saved data and store it as a GameSave object
            gameSave = (GameSave)bf.Deserialize(file);

            // Iterate through all iSaveable objects
            for (int i = iSaveableObjects.Count - 1; i > -1; i--)
            {
                // Check if the game save data contains the ISaveableID
                if (gameSave.gameObjectData.ContainsKey(iSaveableObjects[i].ISaveableID))
                {
                    // If so, restore the data
                    iSaveableObjects[i].ISaveableLoad(gameSave);
                }
                else
                {
                    // Else, destroy the object
                    Component component = (Component)iSaveableObjects[i];
                    Destroy(component.gameObject);
                }
            }

            // Close the file stream
            file.Close();
        }

        // Close the pause menu
        UIManager.Instance.DisablePauseMenu();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SaveDataToFile()
    {
        gameSave = new GameSave();

        // Iterate through all of the iSaveableObjects
        foreach (ISaveable iSaveableObject in iSaveableObjects)
        {
            // Generate the object's save data and store it in the game save dictionary
            gameSave.gameObjectData.Add(iSaveableObject.ISaveableID, iSaveableObject.ISaveableSave());
        }

        BinaryFormatter bf = new BinaryFormatter();

        // Open a file stream to create the save file
        FileStream file = File.Open(Application.persistentDataPath + "/MapleCorners.dat", FileMode.Create);

        // Serialize the game save data into the file
        bf.Serialize(file, gameSave);

        // Close the file
        file.Close();

        // Close the pause screen
        UIManager.Instance.DisablePauseMenu();
    }
}
