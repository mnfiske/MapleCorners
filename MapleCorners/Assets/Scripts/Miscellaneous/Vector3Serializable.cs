// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unity's Vector3 class isn't serialiable, so we need to create our own
[System.Serializable]
public class Vector3Serializable
{
  public float x;
  public float y;
  public float z;

  public Vector3Serializable(float x, float y, float z)
  {
    this.x = x;
    this.y = y;
    this.z = z;
  }

  public Vector3Serializable()
  {

  }
}
