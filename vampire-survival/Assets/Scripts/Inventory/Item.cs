using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int itemHeld;
    public Sprite itemIcon;

    public virtual void Use()
    {
        //Debug.Log("Using " + itemName);
        switch (itemName)
        {
            case "生命药水":
                PlayerHealthMgr.Instance.maxHealth += 5;
                PlayerHealthMgr.Instance.currentHealth += 5;
                //Debug.Log("maxHealth:"+ PlayerHealthMgr.Instance.maxHealth+ "currentHealth:"+ PlayerHealthMgr.Instance.currentHealth);
                PlayerHealthMgr.Instance.healthSlider.maxValue = PlayerHealthMgr.Instance.maxHealth;
                PlayerHealthMgr.Instance.healthSlider.value = PlayerHealthMgr.Instance.currentHealth;
                break;
            case "速度药水":
                PlayeManager.instance.speed += 0.2f;
                Debug.Log("speed:" + PlayeManager.instance.speed);
                break;
            case "攻击药水":
                PlayeManager.instance.perDamage += 1;
                Debug.Log("speed:" + PlayeManager.instance.perDamage);
                break;
            default:
                Debug.Log("No such item");
                break;
        }
    }
}
