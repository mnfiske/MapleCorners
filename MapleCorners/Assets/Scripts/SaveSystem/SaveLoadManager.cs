// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton 
/// </summary>
public class SaveLoadManager : SingletonMonoBehavior<SaveLoadManager>
{
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
}
