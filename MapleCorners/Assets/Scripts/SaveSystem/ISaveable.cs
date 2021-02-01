// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
  string ISaveableID { get; set; }

  GameObjectSave GameObjectSave { get; set; }

  void ISavableRegister();

  void ISavableDeregister();

  void ISavableStoreScene();

  void ISavableRestoreSceme();
}
