using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//public class PropPrefab
//{
//    public GameObject prefab;
//    [Range(0f, 100f)] public float Percentafe;
//}
public class ExpController : MonoBehaviour
{
    public ExpP pickups;

    public static ExpController instance;

    public int currentExp;

    public List<int> expLevels;
    public int currentLevel = 1, levelCounter = 100;

    public List<WeaponsF> weaponToUpgrade;

    public GameObject[] prefabs;
    public float Percentage;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Percentage = 50f;
        while (expLevels.Count < levelCounter)
        {
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count - 1] * 1.1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddExp(int exp)
    {
        currentExp += exp;
        if (currentExp >= expLevels[currentLevel])
        {
            LevelUp();
        }
        UIController.instance.UpdateExp(currentExp, expLevels[currentLevel], currentLevel);

    }

    public void SpawnPickup(Vector3 position,int exp)
    {
        Instantiate(pickups, position, Quaternion.identity).expValue=exp;
    }

    public void SpawnItems(Vector3 position)
    {
        Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.6f, 0.8f), Random.Range(-0.7f, 0.9f));
        if (Random.Range(0, 100) < Percentage)
        {
            int index=Random.Range(0, prefabs.Length);
            Instantiate(prefabs[index], position+offset, Quaternion.identity);
        }
    }

    private void LevelUp()
    {
        currentExp -= expLevels[currentLevel];
        currentLevel++;
        if (currentLevel >= expLevels.Count)
        {
            currentLevel = expLevels.Count - 1;
        }

        //PlayeManager.instance.activeWeapon.LevelUp();

        UIController.instance.levelUpPanel.SetActive(true);

        Time.timeScale = 0f;

        //UIController.instance.levelUpBtns[1].UpdateButtonDisplay(PlayeManager.instance.activeWeapon);
        //UIController.instance.levelUpBtns[0].UpdateButtonDisplay(PlayeManager.instance.assignedWeapons[0]);

        //UIController.instance.levelUpBtns[1].UpdateButtonDisplay(PlayeManager.instance.unassignedWeapons[0]);
        //UIController.instance.levelUpBtns[2].UpdateButtonDisplay(PlayeManager.instance.unassignedWeapons[1]);
        weaponToUpgrade.Clear();

        List<WeaponsF> availableWeapons = new List<WeaponsF>();
        availableWeapons.AddRange(PlayeManager.instance.assignedWeapons);

        if (availableWeapons.Count > 0)
        {
            int selected = Random.Range(0, availableWeapons.Count);
            weaponToUpgrade.Add(availableWeapons[selected]);
            availableWeapons.RemoveAt(selected);
        }

        if (PlayeManager.instance.assignedWeapons.Count + PlayeManager.instance.fullyLevelledWeapons.Count < PlayeManager.instance.maxWeapon)
        {
            availableWeapons.AddRange(PlayeManager.instance.unassignedWeapons);

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
            UIController.instance.levelUpBtns[i].UpdateButtonDisplay(weaponToUpgrade[i]);
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
        //PlayerStatesController.instance.UpdateDisplay();
    }
}
