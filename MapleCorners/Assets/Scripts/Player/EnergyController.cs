using UnityEngine;
using UnityEngine.UI;

public class EnergyController : SingletonMonoBehavior<EnergyController>
{
    public float playerEnergy;
    //Serialize will make it visible in the Unity inspector, but it's still private to this script
    [SerializeField] private Text energyText;

    private void Start()
    {
        // Set Initial Energy Level
        playerEnergy = 5f;

        UpdateEnergy();
    }
    public void UpdateEnergy()
    {
        // this will update the UI text box that displays energy and set it to the player's energy (converted from float to string)
        energyText.text = playerEnergy.ToString("0");
    }

    public void SetEnergy(float newEnergy)
    {
        playerEnergy = newEnergy;
        UpdateEnergy();
    }

    public float GetEnergy()
    {
        return playerEnergy;
    }
}
