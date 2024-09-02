using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsF : MonoBehaviour
{
    public List<WeaponStat> stats;
    public int weaponLevel;
    [HideInInspector]
    public bool isStatsUpdated;

    public Sprite icon;

    public void LevelUp()
    {
        if (weaponLevel < stats.Count - 1)
        {
            weaponLevel++;
            isStatsUpdated = true;
            if (weaponLevel >= stats.Count - 1)
            {
                PlayeManager.instance.fullyLevelledWeapons.Add(this);
                PlayeManager.instance.assignedWeapons.Remove(this);
            }
        }
    }
}
[System.Serializable]
public class WeaponStat
{
    public float speed;
    public float damage;
    public float range;
    public string name;
    //public float timeBetweenShots;
    //public float amount;
    //public float duration;
    public string upgradeText;
}