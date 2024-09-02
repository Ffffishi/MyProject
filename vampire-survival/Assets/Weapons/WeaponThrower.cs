using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponThrower : Weapon
{
    public TakeDamageToEnemy damager;
    private float throwCounter;
    // Start is called before the first frame update
    void Start()
    {
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStatsUpdated == true)
        {
            isStatsUpdated = false;
            SetStats();
        }
        throwCounter -= Time.deltaTime;
        if (throwCounter <= 0f)
        {
            throwCounter = stats[weaponLevel].timeBetweenShots;

            for (int i = 0; i < stats[weaponLevel].amount; i++)
            {
                
                Instantiate(damager, damager.transform.position, damager.transform.rotation).gameObject.SetActive(true);
            }
        }
    }
    void SetStats()
    {
        damager.damage = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;

        damager.transform.localScale = stats[weaponLevel].range * Vector3.one;

        throwCounter = 0f;
    }
}
