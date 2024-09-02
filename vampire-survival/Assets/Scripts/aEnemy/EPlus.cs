using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EPlus : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed;       // 移动速度

    private float timer;
    [HideInInspector] public AIPath aiPath;
    [HideInInspector] public AIDestinationSetter destinationSetter;
    [HideInInspector] public float distanceToPlayer;
    public int expToGive;
    private float birthTime;
    [Header("属性")]
    [SerializeField] public float maxHealth;
    [HideInInspector] public float currentHealth;

    private Transform player;//玩家位置
    private Transform randomTarget;//随机移动目标
    public float attackDe;//攻击间隔
    private float attackTimer;//攻击计时器
    [Header("攻击方式")]
    public GameObject bulletPrefab; // 子弹的Prefab
    public Transform firePoint; // 发射子弹的位置
    public float fireRate = 1f; // 子弹的发射频率（每秒发射几次）
    public float bulletSpeed = 10f; // 子弹的速度
    //private float nextFireTime = 0f;

    [Header("漫游的位置")]
    public Transform[] points;

    //漫游计时器
    private float mTimer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mTimer = 2f;
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //初始化血量及血量ui
        currentHealth = maxHealth;
        //初始化追击目标
        if (destinationSetter.target == null)
        {
            destinationSetter.target = player;
        }
        birthTime = 0f;
    }

    void Update()
    {
        birthTime+=Time.deltaTime;
        GetPlayerDistance();
        RandomMove();
    }
    void FixedUpdate()
    {
        FirstBehaviour();

            Attack2();
        
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
                destinationSetter.target = randomTarget;
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


    //二阶段攻击方式
    private void Attack2()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0&&birthTime>1f)
        {
            Shoot();
            attackTimer = attackDe;
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
    #endregion
    public void GetPlayerDistance()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
    }

    #region 受伤死亡
    public void TakeDamage(float damage)
    {
        currentHealth -= (damage + PlayeManager.instance.perDamage);
        //healthSlider.value = currentHealth;

        //OnHurt?.Invoke();//受伤回调
        if (currentHealth <= 0)
        {
            //OnDie?.Invoke();
            Die();
            ExpController.instance.SpawnPickup(transform.position, expToGive);
            ExpController.instance.SpawnItems(transform.position);

        }
        DNController.instance.SpwanDamageNumber(transform.position, damage + PlayeManager.instance.perDamage);
    }

    private void Die()
    {
        LevelMgr.instance.enemyCounter++;

        Destroy(gameObject);
    }
    #endregion
}
