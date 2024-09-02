using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelGradeBtn : MonoBehaviour
{
    //public TMP_Text nameLevelText, upgradeDescText;
    public Text nameLevelText, upgradeDescText;
    public Image weaponIcon;

    private WeaponsF assingedWeapon;
    public void UpdateButtonDisplay(WeaponsF theWeapon)
    {
        if (theWeapon.gameObject.activeSelf == true)
        {
            upgradeDescText.text = theWeapon.stats[theWeapon.weaponLevel].upgradeText;
            nameLevelText.text = theWeapon.stats[theWeapon.weaponLevel].name + " Lv." + theWeapon.weaponLevel;
            weaponIcon.sprite = theWeapon.icon;
        }
        else
        {
            //upgradeDescText.text = "Unlocked" + theWeapon.stats[theWeapon.weaponLevel].name;
            upgradeDescText.text = "Î´½âËø";
            nameLevelText.text = theWeapon.stats[theWeapon.weaponLevel].name;
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
                PlayeManager.instance.AddWeapon(assingedWeapon);
            }
            UIController.instance.levelUpPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
