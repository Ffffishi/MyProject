using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : WeaponsF
{
    public SpriteRenderer spriteRenderer;

    public EnemyDamage damager;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform rotaionPoint;
    public float bulletSpeed = 10f;

    public float weaponRange;
    public LayerMask whatIsEnemy;

    public float fireRate = 0.1f;     // 子弹发射频率（每秒发射子弹的次数）
    public float burstDuration = 1f;  // 连续发射的持续时间

    public float fireDe = 0.2f;
    private float fireTimer;
    void Start()
    {
        // 设置一个初始偏移量，例如在玩家的右上方
        //transform.position = player.position + offset;
        fireTimer = 0f;
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        SetStat();
    }



    void Update()
    {
        fireTimer += Time.deltaTime;
        //if (fireTimer >= fireDe)
        //{
        //    Fire();

        //    fireTimer = 0f;
        //}
        FindEnemy();
        //更新武器的属性
        if (isStatsUpdated == true)
        {
            isStatsUpdated = false;
            SetStat();
        }

    }

    public void FindEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange, whatIsEnemy);
        if (enemies.Length > 0&& fireTimer>=fireDe)
        {
            Vector3 targetPos = enemies[Random.Range(0, enemies.Length)].transform.position;
            Vector3 direction;
            if (spriteRenderer.flipX == true)
            {
                 direction = (transform.position-targetPos);
            }
            else
            {
                 direction = (targetPos - transform.position);
            }
            //.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //angle -= 90;
            if (angle >= 100 || angle <= -100)
            {
                spriteRenderer.flipY = true;
            }
            else
            {
                spriteRenderer.flipY = false;
            }
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            InvokeRepeating("Fire", 0f, fireRate);
            Invoke("StopFiring", burstDuration);
            //Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);

            fireTimer = 0f;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 bulletDirection = (firePoint.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
    }

    void StopFiring()
    {
        CancelInvoke("Fire");
    }


    /// <summary>
    /// 设置武器的属性
    /// </summary>
    public void SetStat()
    {
        damager.damage = stats[weaponLevel].damage;
    }
}
