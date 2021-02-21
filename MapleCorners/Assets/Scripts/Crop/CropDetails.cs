// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropDetails
{
    [ItemCodeDescription]
    // ID for the seed
    public int seedItemCode;
    // Holds the number of days for the growth stages
    public int[] growthDays;
    // Total number of days it will take the seed to grow
    public int totalGrowthDays;
    // Holds the growth prefabs
    public GameObject[] growthPrefab;
    // Holds the sprites for each growth stage
    public Sprite[] growthSprite;
    // The sprite for the crop that gets harvested
    public Sprite harvestedSprite;
    [ItemCodeDescription]
    // If a crop transforms when harvested, holds the item code for the destination crop
    public int harvestedTransformItemCode;
    // Whether to disable the crop before the harvest anination
    public bool hideCropBeforeHarvestedAnimation;
    // Whether to disable the crop colliders before the harvest animation
    public bool disableCropCollidersBeforeHarvestedAnimation;
    // Whether to play a harvested animation on the final growth stage prefab
    public bool isHarvestedAnimation;
    // Whether there is a harvest action effect
    public bool isHarvestActionEffect = false;
    // Whether the crop should be spawned at the player position
    public bool spawnCropProducedAtPlayerPosition;
    // The harvest action effect
    //public HarvestActionEffect harvestActionEffect;

    // Array of which tools can be used to harvest a crop
    [ItemCodeDescription]
    public int[] harvestToolItemCode;
    // The number of harvest actions for the tool
    public int[] requiredHarvestActions;
    // Array of item codes for the harvested crop
    [ItemCodeDescription]
    public int[] cropProducedItemCode;
    // Minimum number of crops produced by harvest
    public int[] cropProducedMinQuantity;
    // Minimum number of crops produced by harvest
    public int[] cropProducedMaxQuantity;

    /// <summary>
    /// Returns whether the passed in tool can be used to harvest the crop
    /// </summary>
    /// <param name="toolItemCode"></param>
    /// <returns></returns>
    public bool CanUseToolToHarvestCrop(int toolItemCode)
    {
        if (RequiredHarvestActionsForTool(toolItemCode) == -1)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    /// <summary>
    /// Returns the number of harvest actions needed to harvest the crop with the given tool. Returns -1 if the tool cannot harvest the crop
    /// </summary>
    /// <param name="toolItemCode"></param>
    /// <returns></returns>
    public int RequiredHarvestActionsForTool(int toolItemCode)
    {
        for (int i = 0; i < harvestToolItemCode.Length; i++)
        {
            if (harvestToolItemCode[i] == toolItemCode)
            {
                return requiredHarvestActions[i];
            }
        }
        return -1;
    }
}
