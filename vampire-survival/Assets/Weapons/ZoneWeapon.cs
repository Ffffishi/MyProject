using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWeapon : Weapon
{
    public TakeDamageToEnemy damager;
    private float spawnTime,spawnCounter;
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
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0f)
        {
            spawnCounter = spawnTime;
            Instantiate(damager, damager.transform.position,Quaternion.identity,transform).gameObject.SetActive(true);
        }
    }
    public void SetStats()
    {
        damager.damage = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;

        damager.timeBetweenDamage=stats[weaponLevel].speed;
        damager.transform.localScale = stats[weaponLevel].range * Vector3.one;

        spawnTime = stats[weaponLevel].timeBetweenShots;
        spawnCounter = 0f;
    }
}
