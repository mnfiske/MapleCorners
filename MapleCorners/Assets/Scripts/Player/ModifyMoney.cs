// this script will need to be attached to whatever object will modify money
using UnityEngine;

public class ModifyMoney : MonoBehaviour
{
    // Setting up an example for when we have objects to actually modify money (selling/buying)
    
    [SerializeField] private float itemPrice;
    [SerializeField] private MoneyAttribute moneyController;

    // insert some trigger for money to be modified (ex:clicking a buy button in shop)
    // if that is triggered, call UpdateMoney

    void UpdateMoney()
    {
        // update money variable
        moneyController.playerMoney = moneyController.playerMoney - itemPrice;

        // update UI to display new money
        moneyController.UpdateMoney();
    }
}
