using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Attaches to player and looks for collision trigger event
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player collides with item then this component is valid
        Item item = collision.GetComponent<Item>();

        if (item != null)
        {
            // get item details from manager
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);

            // Validate - print description in console if there is a collision with item
            Debug.Log(itemDetails.itemDescription);
        }
    }
}
