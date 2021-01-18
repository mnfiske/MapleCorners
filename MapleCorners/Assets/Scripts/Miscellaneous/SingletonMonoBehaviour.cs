// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Using the British spelling because it's inheriting Unity.MonoBehaviour and I feel like mixing the spellings would be confusing
public abstract class SingletonMonoBehavior<T> : MonoBehaviour where T:MonoBehaviour
{
  private static T instance;

  // Allows outside classes to get our private instance variable
  public static T Instance
  {
    get
    {
      return instance;
    }
  }

  // Awake will run when the game object is initialized
  protected virtual void Awake()
  {
    // If we don't already have an instance of this singleton object, then set the instance to this object
    if (instance == null)
    {
      instance = this as T;
    }
    // If there is already an instance of this singleton object then destory this gameObject--there can only be one
    else
    {
      Destroy(gameObject);
    }
  }
}
