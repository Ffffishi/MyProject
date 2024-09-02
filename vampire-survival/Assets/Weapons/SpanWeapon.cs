using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanWeapon : Weapon
{
    public float rotateSpeed;
    public Transform holder,fireballToSpawn;
    public float timeToSpwan;
    private float spawnCounter;
    public TakeDamageToEnemy damager;

    void Start()
    {
        SetStats();
        //UIController.instance.levelUpBtns[0].UpdateButtonDisplay(this);
    }

    // Update is called once per frame
    void Update()
    {
        //holder.rotation = Quaternion.Euler(0, 0, holder.rotation.eulerAngles.z + rotateSpeed * Time.deltaTime);
        holder.rotation = Quaternion.Euler(0, 0, holder.rotation.eulerAngles.z + rotateSpeed * Time.deltaTime*stats[weaponLevel].speed);
        spawnCounter-=Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = timeToSpwan;
            //Instantiate(fireballToSpawn, fireballToSpawn.position, fireballToSpawn.rotation, holder).gameObject.SetActive(true);
            for(int i = 0; i < stats[weaponLevel].amount; i++)
            {
                float rot = (360 / stats[weaponLevel].amount)*i;
                Instantiate(fireballToSpawn, fireballToSpawn.position, Quaternion.Euler(0,0,rot), holder).gameObject.SetActive(true);
            }

        }
        if (isStatsUpdated==true)
        {
            isStatsUpdated = false;
            SetStats();
        }
    }
    public void SetStats()
    {
        damager.damage = stats[weaponLevel].damage;
        transform.localScale = stats[weaponLevel].range*Vector3.one;
        timeToSpwan = stats[weaponLevel].timeBetweenShots;
        damager.lifeTime = stats[weaponLevel].duration;

        spawnCounter = 0f;
    }
}
