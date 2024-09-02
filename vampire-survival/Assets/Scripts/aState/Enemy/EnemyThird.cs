using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThird : Enemy
{
    [Header("漫游的位置")]
    public Transform[] points;
    private Transform randomTarget;//随机移动目标
    [Header("攻击方式")]
    public GameObject bulletPrefab; // 子弹的Prefab
    public Transform firePoint; // 发射子弹的位置
    public float fireRate = 1f; // 子弹的发射频率（每秒发射几次）
    public float bulletSpeed = 10f; // 子弹的速度
    private float nextFireTime = 0f;
    //private float mTimer=1f;

    //设计一个第三种类型的敌人，攻击方式为发射远程子弹

    //private void FixedUpdate()
    //{
    //    //base.FixedUpdate();
    //    //if (currentHealth <= 0)
    //    //{
    //    //    Destroy(gameObject);
    //    //}
    //    //birthTime += Time.fixedDeltaTime; // 记录敌人出生的时间
    //}
    public override void Attack()
    {
        if (aiPath.enabled == false)
            aiPath.enabled = true;
        //aiPath.canMove = false; // 停止移动
        if (cooldownTimer > 0)
        {
            //    Debug.Log("冷却中" + cooldownTimer);
            cooldownTimer -= Time.deltaTime;
            TransitionToState(EnemyState.Chase);
            return;
        }
        //Debug.Log("attack");

        // 如果当前时间大于下次发射时间
        if (Time.time >= nextFireTime&&birthTime>=1f)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate; // 设置下次发射的时间
            cooldownTimer = cooldownTime; // 开始冷却
        }

    }

    private void Shoot()
    {
        // 创建子弹
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // 设置子弹的方向和速度
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 direction = (player.position - firePoint.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    public override void StopChase()
    {
        //mTimer -= Time.deltaTime;
        if (enemythirdTimer <= 0)
        {
            randomTarget = points[Random.Range(0, points.Length)];
            enemythirdTimer = 1f;
        }
        //aiPath.enabled = !(aiPath.enabled); // 停止追踪
        //randomTarget = points[Random.Range(0, points.Length)];
        destinationSetter.target=randomTarget;
    }
}
