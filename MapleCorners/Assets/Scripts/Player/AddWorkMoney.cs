// this script will need to be attached to whatever object will modify money
using UnityEngine;

public class AddWorkMoney : MonoBehaviour
{
    // Setting up an example for when we have objects to actually modify money (selling/buying)

    [SerializeField] private float workMoney;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UpdateMoney();
        }
    }

    // Allow specific amounts to be added to money
    private void UpdateMoney()
    {
        // update money variable
        MoneyAttribute.Instance.playerMoney += workMoney;

        // update UI to display new money
        MoneyAttribute.Instance.UpdateMoney();
    }
}
