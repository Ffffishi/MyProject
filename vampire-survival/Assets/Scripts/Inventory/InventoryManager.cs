using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Inventory myBag;

    public GameObject slotGrid;

    public Slot itemPrefab;

    public Text itemInfo;

    public Button useButton;

    public static Item selectItem;

    private void OnEnable()
    {
        RefreshItem();
        instance.itemInfo.text = "";
        useButton.onClick.AddListener(OnUseButtonClick);
    }

    public static void SetSelectItem(Item item)
    {
        selectItem=item;
    }

    private void OnUseButtonClick()
    {
        if (selectItem != null)
        {
            selectItem.Use();
            selectItem.itemHeld --;
            if (selectItem.itemHeld <= 0)
            {
                instance.myBag.itemList.Remove(selectItem);
            }
            RefreshItem();
        }
    }


    public static void UpdateItemInfo(string info)
    {
        instance.itemInfo.text = info;
    }

    public static void CreatNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.itemPrefab, instance.slotGrid.transform);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);

        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemIcon;
        newItem.slotNumber.text = item.itemHeld.ToString();
    }

    public static void RefreshItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            if (instance.slotGrid.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            CreatNewItem(instance.myBag.itemList[i]);
        }
    }
}
