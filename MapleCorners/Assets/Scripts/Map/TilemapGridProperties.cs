// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TilemapGridProperties : MonoBehaviour
{
  private Tilemap tilemap;
  //private Grid grid;
  [SerializeField] private SO_GridProperties gridProperties = null;
  [SerializeField] private GridBoolProperty gridBoolProperty = GridBoolProperty.diggable;

  private void OnEnable()
  {
    // Don't populate if the game is being played--only populate in the editor
    if (!Application.IsPlaying(gameObject))
    {
      // Cache the value of the Tilemap compnent in the tilemap field
      tilemap = GetComponent<Tilemap>();

      // If the gridProperties scriptable object has been passed in, clear any items in the GridPropertyList
      if (gridProperties != null)
      {
        gridProperties.GridPropertyList.Clear();
      }
    }
  }

  /// <summary>
  /// When we disable, look thru the tilemap & capture the state to save to the scriptable object
  /// </summary>
  private void OnDisable()
  {
    if (!Application.IsPlaying(gameObject))
    {
      UpdateGridProperties();

      if (gridProperties != null)
      {
        // This is required to ensure that the updated gridproperties gameobject gets saved when the game is saved - otherwise they are not saved.
        EditorUtility.SetDirty(gridProperties);
      }
    }
  }

  /// <summary>
  /// Captures everything that has been painted on the tilemap
  /// </summary>
  private void UpdateGridProperties()
  {
    // Compress the bounds of the tilemap to the painted tiles
    tilemap.CompressBounds();

    if (!Application.IsPlaying(gameObject))
    {
      // Check if the gridProperties scriptable object has been passed in
      if (gridProperties != null)
      {
        // Set the bounds of the tilemap
        Vector3Int startCell = tilemap.cellBounds.min;
        Vector3Int endCell = tilemap.cellBounds.max;

        // Go through every square in the tilemap, check if it's null. If not, add a true value to the GridPropertyList
        for (int x = startCell.x; x < endCell.x; x++)
        {
          for (int y = startCell.y; y < endCell.y; y++)
          {
            TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));

            if (tile != null)
            {
              gridProperties.GridPropertyList.Add(new GridProperty(new GridCoordinate(x, y), gridBoolProperty, true));
            }
          }
        }
      }
    }
  }

    // Update is called once per frame
  private void Update()
  {
    if (!Application.IsPlaying(gameObject))
      {
        Debug.Log("Diable property tilemaps");
      }
  }
}
