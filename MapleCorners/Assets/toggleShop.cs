using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleShop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // get the parent game object since we need to reference an inactive object
            GameObject obj = GameObject.FindWithTag("ShopCanvas");

            // get the actual shop, which is the child game object of the parent
            GameObject shop = obj.transform.Find("ShopCanvas").gameObject;
            shop.SetActive(true);

            // access the toggle button and set it to active
            // this makes the shop button visible when the shop is open
            GameObject shopToggle = obj.transform.Find("ToggleShop").gameObject;
            shopToggle.SetActive(true);
        }
    }
}
