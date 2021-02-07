// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for saveable items
/// </summary>
public interface ISaveable
{
  /// <summary>
  /// Unique ID property
  /// </summary>
  string ISaveableID { get; set; }

  /// <summary>
  /// Object to store the save data for the game object
  /// </summary>
  GameObjectSave GameObjectSave { get; set; }

  /// <summary>
  /// Registers the game object with the SaveLoadManager
  /// </summary>
  void ISaveableRegister();

  /// <summary>
  /// De-registers the game object from the SaveLoadManager
  /// </summary>
  void ISaveableDeregister();

  /// <summary>
  /// Stores the scene data
  /// </summary>
  /// <param name="name"></param>
  void ISaveableStoreScene(string name);

  /// <summary>
  /// Restores the scene data
  /// </summary>
  /// <param name="name"></param>
  void ISaveableRestoreScene(string name);
}
