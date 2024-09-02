using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXLevelController : MonoBehaviour
{
    public static EXLevelController instance;
    public EXPPickup pickup;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public int currentExperience;

    public List<int> expLevels;
    public int currentLevel = 1,levelCounter = 100;

    public List<Weapon> weaponToUpgrade;
    /// <summary>
    /// old
    /// </summary>
    void Start()
    {
        while (expLevels.Count < levelCounter)
        {
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count-1] * 1.1f));
        }
    }

    public void AddExp(int experience)
    {
        currentExperience += experience;
        if(currentExperience >= expLevels[currentLevel]){
            LevelUp();
        }
        UIController.instance.UpdateExp(currentExperience, expLevels[currentLevel], currentLevel);
    }

    public void SpawnExp(Vector3 position,int expValue)
    {
        Instantiate(pickup, position, Quaternion.identity).expValue=expValue;
    }
    private void LevelUp()
    {
        currentExperience -= expLevels[currentLevel];
        currentLevel++;
        if(currentLevel >= expLevels.Count)
        {
            currentLevel = expLevels.Count-1;
        }
        //PlayerController.instance.activeWeapon.LevelUp();
        UIController.instance.levelUpPanel.SetActive(true);
        Time.timeScale = 0f;
        //UIController.instance.levelUpBtns[1].UpdateButtonDisplay(PlayerController.instance.activeWeapon);
        //UIController.instance.levelUpBtns[0].UpdateButtonDisplay(PlayerController.instance.assignedWeapons[0]);

        //UIController.instance.levelUpBtns[1].UpdateButtonDisplay(PlayerController.instance.unassignedWeapons[0]);
        //UIController.instance.levelUpBtns[2].UpdateButtonDisplay(PlayerController.instance.unassignedWeapons[1]);
        weaponToUpgrade.Clear();
        
        List<Weapon> availableWeapons = new List<Weapon>();
        availableWeapons.AddRange(PlayerController.instance.assignedWeapons);

        if(availableWeapons.Count>0)
        {
            int selected=Random.Range(0,availableWeapons.Count);
            weaponToUpgrade.Add(availableWeapons[selected]);
            availableWeapons.RemoveAt(selected);
        }
        if (PlayerController.instance.assignedWeapons.Count+PlayerController.instance.fullyLevelledWeapons.Count < PlayerController.instance.maxWeapon)
        {
            availableWeapons.AddRange(PlayerController.instance.unassignedWeapons);

        }
        for (int i = weaponToUpgrade.Count; i < 3; i++)
        {
            if (availableWeapons.Count > 0)
            {
                int selected = Random.Range(0, availableWeapons.Count);
                weaponToUpgrade.Add(availableWeapons[selected]);
                availableWeapons.RemoveAt(selected);
            }
        }
        for (int i = 0; i < weaponToUpgrade.Count; i++)
        {
            //UIController.instance.levelUpBtns[i].UpdateButtonDisplay(weaponToUpgrade[i]);
        }
        for (int i = 0; i < UIController.instance.levelUpBtns.Length; i++)
        {
            if (i < weaponToUpgrade.Count)
            {
                UIController.instance.levelUpBtns[i].gameObject.SetActive(true);
            }
            else
            {
                UIController.instance.levelUpBtns[i].gameObject.SetActive(false);
            }
        }
        PlayerStatesController.instance.UpdateDisplay();
    }
}
