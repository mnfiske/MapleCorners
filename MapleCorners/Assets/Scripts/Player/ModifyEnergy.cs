// Code is referenced and adapted from a youtube tutorial on implementing a health system
// 2D Health System for Unity - SpeedTutor (2019). Available at https://www.youtube.com/watch?v=tzEVJ3tKQUg

using UnityEngine;

public class ModifyEnergy : MonoBehaviour
{
    [SerializeField] private float bedEnergy;
    [SerializeField] private EnergyAttribute energyController;

    // if the player object collides with the bed object, it will reset the player's energy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ResetEnergy();
        }
    }

    // this will reset energy to full stats (100)
    void ResetEnergy()
    {
        // update energy variable
        energyController.playerEnergy = 100;

        // update UI to display new energy
        energyController.UpdateEnergy();
    }

    //
}
