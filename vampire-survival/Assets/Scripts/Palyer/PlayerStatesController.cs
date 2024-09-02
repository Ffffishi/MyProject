using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesController : MonoBehaviour
{
    public static PlayerStatesController instance;
    void Awake()
    {
        instance = this;
    }

    public List<PlayerStatValue> moveSpeed, health, pickUpRange, maxWeapons;

    public int moveSpeedLevelCount, healthLevelCount, pickUpRangeLevelCount;

    public int moveSpeedLevel, healthLevel, pickUpRangeLevel, maxWeaponsLevel;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = moveSpeed.Count - 1; i < moveSpeedLevelCount; i++)
        {
            moveSpeed.Add(new PlayerStatValue(moveSpeed[i].cost + moveSpeed[1].cost, moveSpeed[i].value + moveSpeed[1].value-moveSpeed[0].value));
        }

        for (int i = health.Count - 1; i < healthLevelCount; i++)
        {
            health.Add(new PlayerStatValue(health[i].cost + health[1].cost, health[i].value + health[1].value - health[0].value));
        }

        for (int i = pickUpRange.Count - 1; i < pickUpRangeLevelCount; i++)
        {
            pickUpRange.Add(new PlayerStatValue(pickUpRange[i].cost + pickUpRange[1].cost, pickUpRange[i].value + pickUpRange[1].value - pickUpRange[0].value));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UIController.instance.levelUpPanel.activeSelf == true)
        {
            UpdateDisplay();
        }
    }

    public void UpdateDisplay()
    {
        if (moveSpeedLevel < moveSpeed.Count - 1)
        {
            UIController.instance.moveSpeedDisplay.UpdateDisplay(moveSpeed[moveSpeedLevel + 1].cost, moveSpeed[moveSpeedLevel].value, moveSpeed[moveSpeedLevel + 1].value);
        }
        else
        {
            UIController.instance.moveSpeedDisplay.ShowMaxLevel();
        }

        if (healthLevel < health.Count - 1)
        {
            UIController.instance.healthDisplay.UpdateDisplay(health[healthLevel + 1].cost, health[healthLevel].value, health[healthLevel + 1].value);
        }
        else
        {
            UIController.instance.healthDisplay.ShowMaxLevel();
        }

        if (maxWeaponsLevel < maxWeapons.Count - 1)
        {
            UIController.instance.maxWeaponsDisplay.UpdateDisplay(maxWeapons[maxWeaponsLevel + 1].cost, maxWeapons[maxWeaponsLevel].value, maxWeapons[maxWeaponsLevel + 1].value);
        }
        else
        {
            UIController.instance.maxWeaponsDisplay.ShowMaxLevel();
        }

        if (pickUpRangeLevel < pickUpRange.Count - 1)
        {
            UIController.instance.pickupDisplay.UpdateDisplay(pickUpRange[pickUpRangeLevel + 1].cost, pickUpRange[pickUpRangeLevel].value, pickUpRange[pickUpRangeLevel + 1].value);
        }
        else
        {
            UIController.instance.pickupDisplay.ShowMaxLevel();
        }
    }

    public void PurchaseMoveSpeed()
    {
        moveSpeedLevel++;
        CoinController.instance.SpendCoin(moveSpeed[moveSpeedLevel].cost);
        UpdateDisplay();
        
        PlayerController.instance.moveSpeed = moveSpeed[moveSpeedLevel].value;
    }

    public void PurchaseHealth()
    {
        healthLevel++;
        CoinController.instance.SpendCoin(health[healthLevel].cost);
        UpdateDisplay();

        PlayerHealthController.instance.maxHealth = health[healthLevel].value;
        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
    }

    public void PurchaseMaxWeapons()
    {
        maxWeaponsLevel++;
        CoinController.instance.SpendCoin(maxWeapons[maxWeaponsLevel].cost);
        UpdateDisplay();

        PlayerController.instance.maxWeapon = Mathf.RoundToInt(maxWeapons[maxWeaponsLevel].value);
    }


    public void PurchasePickupRange()
    {   
        pickUpRangeLevel++;
        CoinController.instance.SpendCoin(pickUpRange[pickUpRangeLevel].cost);
        UpdateDisplay();

        PlayerController.instance.pickupRange = pickUpRange[pickUpRangeLevel].value;

    }
}
[System.Serializable]
public class PlayerStatValue
{
    public int cost;
    public float value;

    public PlayerStatValue(int cost, float value)
    {
        this.cost = cost;
        this.value = value;
    }
}