// this script will need to be attached to whatever object will modify money
using UnityEngine;

public class ModifyMoney : MonoBehaviour
{
    // Setting up an example for when we have objects to actually modify money (selling/buying)
    
    [SerializeField] private float itemPrice;
    [SerializeField] private MoneyAttribute moneyController;

    // insert some trigger for money to be modified (ex:clicking a buy button in shop)
    // if that is triggered, call UpdateMoney

    // make public to allow buttons to call function
    public void UpdateMoney()
    {
        if (moneyController.playerMoney >= itemPrice)
        {
            // update money variable
            moneyController.playerMoney = moneyController.playerMoney - itemPrice;

            // update UI to display new money
            moneyController.UpdateMoney();
        }
        else
        {
            // cannot afford item
        }
    }

    public void UpdateMoneySell()
    {

        Item item = GameObject.FindGameObjectWithTag(Tags.SellCorn).GetComponent<Item>();
        //Debug.Log(item);
        //if inventory contains that item, remove it
        if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, item.ItemCode) != -1)
        {
            // update money variable
            moneyController.playerMoney = moneyController.playerMoney - itemPrice;

            // update UI to display new money
            moneyController.UpdateMoney();
        }
    }
 
}
