using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpSelectionBtn : MonoBehaviour
{
    /// <summary>
    /// old
    /// </summary>
    public TMP_Text nameLevelText,upgradeDescText;
    public Image weaponIcon;

    private Weapon assingedWeapon;
    public void UpdateButtonDisplay(Weapon theWeapon)
    {
        if (theWeapon.gameObject.activeSelf == true)
        {
            upgradeDescText.text = theWeapon.stats[theWeapon.weaponLevel].upgradeText;
            nameLevelText.text = theWeapon.name + " Lv." + theWeapon.weaponLevel;
            weaponIcon.sprite = theWeapon.icon;
        }
        else
        {
            upgradeDescText.text = "Unlocked"+theWeapon.name;
            nameLevelText.text = theWeapon.name;
            weaponIcon.sprite = theWeapon.icon;
        }
        

        assingedWeapon = theWeapon;
    }

    public void SelectUpgrade()
    {
        if (assingedWeapon != null)
        {
            if (assingedWeapon.gameObject.activeSelf == true)
            {
                assingedWeapon.LevelUp();
            }
            else
            {
                PlayerController.instance.AddWeapon(assingedWeapon);
            }
            UIController.instance.levelUpPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
