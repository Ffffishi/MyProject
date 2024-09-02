using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : WeaponsF
{
    public EnemyDamage damager;
    public ShovleProj projectile;

    private float shotCounter=1f;

    public float weaponRange;
    public LayerMask whatIsEnemy;

    void Start()
    {
        SetStat();
        //UIController.instance.levelUpBtns[0].UpdateButtonDisplay(this);
    }


    void Update()
    {
        //生成铲子攻击敌人
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            shotCounter = 1f;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange , whatIsEnemy);
            if (enemies.Length > 0)
            {
                for (int i = 0; i < projectile.count; i++)
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
        //更新武器的属性
        if (isStatsUpdated == true)
        {
            isStatsUpdated = false;
            SetStat();
        }
    }

    /// <summary>
    /// 设置武器的属性
    /// </summary>
    public void SetStat()
    {
        damager.damage = stats[weaponLevel].damage;
    }
}
