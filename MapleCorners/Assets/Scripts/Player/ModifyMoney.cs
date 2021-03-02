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

            // if the player purchases the ticket to end the game, display the winning message
            if (itemname == "ticket")
            {
                GameObject endingMessage = GameObject.Find("Ticket");
                GameObject msg = endingMessage.transform.Find("CongratsMessage").gameObject;
                msg.SetActive(true);
            }
            else
            {
                Item item = GameObject.Find(itemname).GetComponent<Item>();

                InventoryManager.Instance.AddBoughtItem(InventoryLocation.player, item, GameObject.Find(itemname));
            }
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
