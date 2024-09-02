using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatUpDisplay : MonoBehaviour
{
    public TMP_Text costText, valueText;
    
    public GameObject upgradeButton;

    public void UpdateDisplay(int cost, float oldValue,float newValue)
    {
        valueText.text = "value:" + oldValue.ToString("F1") + " -> " + newValue.ToString("F1");
        costText.text = "cost:" + cost;

        if (cost <= CoinController.instance.currentCoin)
        {
            upgradeButton.SetActive(true);
        }
        else
        {
            upgradeButton.SetActive(false);
        }
    }

    public void ShowMaxLevel()
    {
        valueText.text = "Max Level ";
        costText.text = "Max Level ";
        upgradeButton.SetActive(false);
    }
}
