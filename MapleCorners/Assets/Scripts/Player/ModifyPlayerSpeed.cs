using UnityEngine;

public class ModifyPlayerSpeed : MonoBehaviour
{
    [SerializeField] private float newRunSpeed =  2.666f;
    [SerializeField] private float newWalkSpeed = 1.333f;

    // if the player object collides with the object, adjust the player's base speed
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Settings.DecMovementSpeed();
        }
    }

    // after leaving collission, reset movement
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Settings.IncMovementSpeed();
        }
    }
}