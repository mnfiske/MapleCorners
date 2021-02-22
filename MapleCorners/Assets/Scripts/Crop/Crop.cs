﻿// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    private int harvestActionCount = 0;

    [HideInInspector]
    public Vector2Int cropGridPosition;

    public void ProcessToolAction(ItemDetails equippedItemDetails, bool isToolRight, bool isToolLeft, bool isToolDown, bool isToolUp)
    {
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cropGridPosition.x, cropGridPosition.y);

        if (gridPropertyDetails == null)
            return;

        ItemDetails seedItemDetails = InventoryManager.Instance.GetItemDetails(gridPropertyDetails.SeedItemCode);
        if (seedItemDetails == null)
            return;

        CropDetails cropDetails = GridPropertiesManager.Instance.GetCropDetails(seedItemDetails.itemCode);
        if (cropDetails == null)
            return;
        harvestActionCount += 1;

        int requiredHarvestActions = cropDetails.RequiredHarvestActionsForTool(equippedItemDetails.itemCode);
        if (requiredHarvestActions == -1)
            return;

        if (harvestActionCount >= requiredHarvestActions)
        {
            HarvestCrop(cropDetails, gridPropertyDetails);
        }
    }


    private void HarvestCrop(CropDetails cropDetails, GridPropertyDetails gridPropertyDetails)
    {
        gridPropertyDetails.SeedItemCode = -1;
        gridPropertyDetails.GrowthDays = -1;
        gridPropertyDetails.DaysSinceLastHarvest = -1;
        gridPropertyDetails.DaysSinceWatered = -1;
        
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.GridX, gridPropertyDetails.GridY, gridPropertyDetails);

        HarvestActions(cropDetails, gridPropertyDetails);
    }


    private void HarvestActions(CropDetails cropDetails, GridPropertyDetails gridPropertyDetails)
    {
        SpawnHarvestedItems(cropDetails);

        Destroy(gameObject);
    }


    private void SpawnHarvestedItems(CropDetails cropDetails)
    {
        for (int i = 0; i < cropDetails.cropProducedItemCode.Length; i++)
        {
            int cropsToProduce;

            if (cropDetails.cropProducedMinQuantity[i] == cropDetails.cropProducedMaxQuantity[i] ||
                cropDetails.cropProducedMaxQuantity[i] < cropDetails.cropProducedMinQuantity[i])
            {
                cropsToProduce = cropDetails.cropProducedMinQuantity[i];
            }
            else
            {
                cropsToProduce = Random.Range(cropDetails.cropProducedMinQuantity[i], cropDetails.cropProducedMaxQuantity[i] + 1);
            }

            for (int j = 0; j < cropsToProduce; j++)
            {
                Vector3 spawnPosition;
                if (cropDetails.spawnCropProducedAtPlayerPosition)
                {
                    InventoryManager.Instance.AddItem(InventoryLocation.player, cropDetails.cropProducedItemCode[i]);
                }
                else
                {
                    spawnPosition = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), 0f);
                    SceneItemsManager.Instance.InstantiateSceneItem(cropDetails.cropProducedItemCode[i], spawnPosition);
                }
            }
        }
    }
}
