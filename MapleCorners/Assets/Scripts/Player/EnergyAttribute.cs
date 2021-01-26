using UnityEngine;
using UnityEngine.UI;

public class EnergyAttribute : MonoBehaviour
{
    public float playerEnergy;
    //Serialize will make it visible in the Unity inspector, but it's still private to this script
    [SerializeField] private Text energyText;

    private void Start()
    {
        UpdateEnergy();
    }
    public void UpdateEnergy()
    {
        // this will update the UI text box that displays energy and set it to the player's energy (converted from float to string)
        energyText.text = playerEnergy.ToString("0");
    }
}
