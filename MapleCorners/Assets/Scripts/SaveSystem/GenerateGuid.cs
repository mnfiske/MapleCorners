// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GenerateGuid : MonoBehaviour
{
  [SerializeField]
  private string _guid = string.Empty;

  public string Guid
  {
    get
    {
      return _guid;
    }
    set
    {
      _guid = value;
    }
  }

  private void Awake()
  {
    if (!Application.IsPlaying(gameObject))
    {
      if (string.IsNullOrEmpty(_guid))
      {
        _guid = System.Guid.NewGuid().ToString();
      }
    }
  }
}
