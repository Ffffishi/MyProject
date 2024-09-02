using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ProjectileThrow : Weapon
{
    public TakeDamageToEnemy damager;
    public Projectile projectile;

    private float shotCounter;

    public float weaponRange;
    public LayerMask whatIsEnemy;
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
        shotCounter-=Time.deltaTime;
        if (shotCounter <= 0f)
        {
            shotCounter=stats[weaponLevel].timeBetweenShots;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange*stats[weaponLevel].range, whatIsEnemy);
            if (enemies.Length > 0)
            {
                for (int i = 0; i < stats[weaponLevel].amount; i++)
                {
                    Vector3 targetPos = enemies[Random.Range(0, enemies.Length)].transform.position;
                    Vector3 direction = (targetPos - transform.position);
                        //.normalized;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    angle -= 90;
                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);
                }
            }
        }
    }

    void SetStats()
    {
        damager.damage = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;

        damager.transform.localScale = stats[weaponLevel].range * Vector3.one;
        shotCounter = 0f;

        projectile.moveSpeed = stats[weaponLevel].speed;
    }
}
