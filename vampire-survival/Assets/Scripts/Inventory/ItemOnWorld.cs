using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{

    public Item item;
    public Inventory inventory;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            //Debug.Log("Player picked up " + item.itemName);
            AddItem();
            Destroy(gameObject);
        }
    }

    public void AddItem()
    {
        if (!inventory.itemList.Contains(item))
        {
            inventory.itemList.Add(item);
            //InventoryManager.CreatNewItem(item);
        }
        else
        {
            item.itemHeld += 1;
        }
        InventoryManager.RefreshItem();
    }
}
