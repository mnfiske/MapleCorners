// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropDetailsList", menuName = "Scriptable Objects/Crop/Crop Details List")]
public class SO_CropDetailsList : ScriptableObject
{
    [SerializeField]
    public List<CropDetails> cropDetails;


    /// <summary>
    /// Returns the crop details for the seed that has the passed in seedItemCode
    /// </summary>
    /// <param name="seedItemCode"></param>
    /// <returns></returns>
    public CropDetails GetCropDetails(int seedItemCode)
    {
        return cropDetails.Find(x => x.seedItemCode == seedItemCode);
    }
}
