using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceNextDay : MonoBehaviour
{
    // if the player object collides with the object, increment time to next day
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Advancing time to next day");
            TimeManager.Instance.AdvanceToNextDay();
        }
    }
}
