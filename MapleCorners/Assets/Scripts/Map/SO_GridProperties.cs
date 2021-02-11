// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_GridProperties", menuName = "Scriptable Objects/Grid Properties")]
public class SO_GridProperties : ScriptableObject
{
  public SceneName SceneName;
  public int GridWidth;
  public int GridHeight;
  public int OriginX;
  public int OriginY;

  [SerializeField]
  public List<GridProperty> GridPropertyList;
}
