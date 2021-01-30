// "Citation: Unity 2D Game Developer Course Farming RPG"

using UnityEngine;
using Cinemachine;

public class SwitchConfineBoundingShape : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    SwitchBoundingShape();
  }

  //Changes the collider that Cinemachine uses to determine the edges of the screen
  private void SwitchBoundingShape()
  {
    //Get the polygon collider on the bounds confiner
    PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.BoundsConfiner).GetComponent<PolygonCollider2D>();

    //Get the cinemachine confiner
    CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();

    //Set the cinemachine confiner's m_BoundingShape2D property to the polygon collider on the bounds confiner
    cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;

    //Clear the cache & reapply the polygon collider
    cinemachineConfiner.InvalidatePathCache();
  }
}