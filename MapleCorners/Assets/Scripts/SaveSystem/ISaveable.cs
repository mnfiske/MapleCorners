// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
  string ISaveableID { get; set; }

  GameObjectSave GameObjectSave { get; set; }

  void ISaveableRegister();

  void ISaveableDeregister();

  void ISaveableStoreScene(string name);

  void ISaveableRestoreScene(string name);
}
