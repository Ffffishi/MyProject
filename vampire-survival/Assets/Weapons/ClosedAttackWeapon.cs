using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedAttackWeapon : Weapon
{
    public TakeDamageToEnemy damager;
    public float attackCounter,direction;
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
        attackCounter-=Time.deltaTime;
        if (attackCounter <= 0f)
        {
            attackCounter = stats[weaponLevel].timeBetweenShots;
            direction = Input.GetAxisRaw("Horizontal");
            if (direction != 0)
            {
                if(direction > 0)
                {
                    damager.transform.rotation = Quaternion.identity;
                }
                else
                {
                    damager.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                }
            }
            Instantiate(damager, damager.transform.position, damager.transform.rotation, transform.parent).gameObject.SetActive(true);
            for (int i = 0; i < stats[weaponLevel].amount; i++)
            {
                float rot = (360 / stats[weaponLevel].amount) * i;
                Instantiate(damager, damager.transform.position, Quaternion.Euler(0, 0,damager.transform.rotation.eulerAngles.z + rot), transform).gameObject.SetActive(true);
            }
        }
    }
    void SetStats()
    {
        damager.damage = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;

        damager.transform.localScale = stats[weaponLevel].range * Vector3.one;

        attackCounter = 0f;
    }
}
