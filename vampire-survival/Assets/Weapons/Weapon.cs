using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<WeaponStats> stats;
    public int weaponLevel;
    [HideInInspector]
    public bool isStatsUpdated;

    public Sprite icon;
    /// <summary>
    /// old
    /// </summary>
    public void LevelUp()
    {
        if (weaponLevel < stats.Count-1)
        {
            weaponLevel++;
             isStatsUpdated=true;
            if (weaponLevel>=stats.Count-1)
            {
                PlayerController.instance.fullyLevelledWeapons.Add(this);
                PlayerController.instance.assignedWeapons.Remove(this);
            }
        }
    }

}

[System.Serializable]
public class WeaponStats
{
    public float speed;
    public float damage;
    public float range;
    public float timeBetweenShots;
    public float amount;
    public float duration;
    public string upgradeText;
}