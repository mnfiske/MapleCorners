// this script will need to be attached to whatever object will modify money
using UnityEngine;

public class ModifyMoney : MonoBehaviour
{
    // Setting up an example for when we have objects to actually modify money (selling/buying)

    [SerializeField] private float workMoney;
    [SerializeField] private float itemPrice;
    [SerializeField] private MoneyAttribute moneyController;
    [SerializeField] private string itemname;

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

            //Item item = GameObject.FindGameObjectWithTag(Tags.shopItem).GetComponent<Item>();
            Item item = GameObject.Find(itemname).GetComponent<Item>();
            //Debug.Log(item);
            InventoryManager.Instance.AddBoughtItem(InventoryLocation.player, item, GameObject.Find(itemname));
        }
        else
        {
            // cannot afford item
        }
    }

    // Allow specific amounts to be added to money
    private void UpdateMoney(float amount)
    {
        // update money variable
        moneyController.playerMoney += amount;

        // update UI to display new money
        moneyController.UpdateMoney();
    }

    // Run from work
    public void GiveWorkMoney()
    {
        UpdateMoney(workMoney);
    }

    public void UpdateMoneySell()
    {

        Item item = GameObject.Find(itemname).GetComponent<Item>();
        //Debug.Log(item);
        //if inventory contains that item, remove it
        if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, item.ItemCode) != -1)
        {
            // update money variable
            moneyController.playerMoney = moneyController.playerMoney - itemPrice;

            // update UI to display new money
            moneyController.UpdateMoney();

            InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);
        }
    }
 
}
