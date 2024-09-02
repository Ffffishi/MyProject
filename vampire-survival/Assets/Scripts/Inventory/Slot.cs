using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item slotItem;

    public Image slotImage;

    public Text slotNumber;

    public void ItemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotItem.itemDescription);
        InventoryManager.SetSelectItem(slotItem);
        //InventoryManager.selectSlot = this.gameObject;
    }

}
