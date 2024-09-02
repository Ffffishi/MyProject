using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BossA : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed;       // 移动速度

    //死亡效果
    public GameObject deathEffect;

    private float timer;
    [HideInInspector] public AIPath aiPath;
    [HideInInspector] public AIDestinationSetter destinationSetter;
    [HideInInspector] public float distanceToPlayer;
    public int expToGive;

    [Header("属性")]
    [SerializeField] public float maxHealth;
    [HideInInspector] public float currentHealth;

    public Slider healthSlider;

    private Transform player;//玩家位置
    private Transform randomTarget;//随机移动目标
    public float attackDe;//攻击间隔
    private float attackTimer;//攻击计时器
    private Transform attackTarget;//产生预制体的地点

    //public Transform pointRightUp, pointLeftDown;
    [Header("产生的敌人")]
    public GameObject[] enemyPrefabs;
    [Header("漫游的位置")]
    public Transform[] points;
    [Header("子弹")]
    public GameObject bulletPrefab;

    public float bulletSpeed;  // 子弹的速度
    public int bulletCount; // 子弹的数量
    float angleStep; // 每个子弹之间的角度间隔
    float angle; // 初始角度
    //漫游计时器
    private float mTimer;
    void Start()
    {
        angleStep = 360f / bulletCount; // 每个子弹之间的角度间隔
        angle = 0f; // 初始角度

        rb = GetComponent<Rigidbody2D>();
        mTimer = 2f;
        aiPath =GetComponent<AIPath>();
        destinationSetter=GetComponent<AIDestinationSetter>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //初始化血量及血量ui
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        //初始化追击目标
        if (destinationSetter.target == null)
        {
            destinationSetter.target = player;
        }

    }

    void Update()
    {
        GetPlayerDistance();
        RandomMove();
    }
    void FixedUpdate()
    {
        FirstBehaviour();
        if (currentHealth/maxHealth > 0.5f)
        {
            Attack();
        }
        else
        {
            Attack2();
        }
    }

    #region AI
    //第一阶段AI
    public void FirstBehaviour()
    {
        //靠近玩家后开始随机小范围移动
        if (distanceToPlayer <= 8f)
        {
            destinationSetter.target = null;
            if (randomTarget != null)
            {
                destinationSetter.target= randomTarget;
            }
        }
        else
        {
            destinationSetter.target = player;
        }
    }

    private void RandomMove()
    {
        mTimer -= Time.deltaTime;
        if (mTimer <= 0)
        {
            randomTarget = points[Random.Range(0, points.Length)];
            mTimer = 1f;
        }
    }
    //一阶段攻击方式
    private void Attack()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            attackTarget = this.transform;

            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], attackTarget.position, Quaternion.identity);

            attackTimer=attackDe;
        }
    }

    //二阶段攻击方式
    private void Attack2()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
                float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
                Vector3 bulletDirection = new Vector3(dirX, dirY, 0f);

                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

                angle += angleStep;
            }
            attackTimer = attackDe;
        }
    }

#endregion
    public void GetPlayerDistance()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
    }

    #region 受伤死亡
    public void TakeDamage(float damage)
    {
        currentHealth -= (damage + PlayeManager.instance.perDamage);
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            Die();
            ExpController.instance.SpawnPickup(transform.position, expToGive);
            ExpController.instance.SpawnItems(transform.position);
            
        }
        DNController.instance.SpwanDamageNumber(transform.position, damage + PlayeManager.instance.perDamage);
    }

    private void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        LevelMgr.instance.enemyCounter++;
        LevelMgr.instance.isKilledBoss = true;
        Destroy(gameObject);

        //Invoke("ShowEnd", 2f);
    }
    #endregion

    //public void ShowEnd()
    //{
    //    UIController.instance.levelEndPanel.SetActive(true);
    //}
}
