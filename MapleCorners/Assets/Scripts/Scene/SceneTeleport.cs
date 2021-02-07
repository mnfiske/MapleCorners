﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTeleport : MonoBehaviour
{
  [SerializeField] private SceneName sceneNameGoto = SceneName.Scene1_Apartment;
  [SerializeField] private Vector3 scenePositionGoto = new Vector3();

    // Trigger when Player walks onto collider
    private void OnTriggerStay2D(Collider2D collision)
  {
    Player player = collision.GetComponent<Player>();

    if (player != null)
    {
        // Get player position. Use either player X unless specified
        float xPosition = Mathf.Approximately(scenePositionGoto.x, 0f) ? player.transform.position.x : scenePositionGoto.x;

        float yPosition = Mathf.Approximately(scenePositionGoto.y, 0f) ? player.transform.position.y : scenePositionGoto.y;

        float zPosition = 0f;   // don't change Z level

            // Teleport to new scene
            SceneControllerManager.Instance.FadeAndLoadScene(sceneNameGoto.ToString(), 
                new Vector3(xPosition, yPosition, zPosition));
    }
  }
}
