using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EPlus : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed;       // �ƶ��ٶ�

    private float timer;
    [HideInInspector] public AIPath aiPath;
    [HideInInspector] public AIDestinationSetter destinationSetter;
    [HideInInspector] public float distanceToPlayer;
    public int expToGive;
    private float birthTime;
    [Header("����")]
    [SerializeField] public float maxHealth;
    [HideInInspector] public float currentHealth;

    private Transform player;//���λ��
    private Transform randomTarget;//����ƶ�Ŀ��
    public float attackDe;//�������
    private float attackTimer;//������ʱ��
    [Header("������ʽ")]
    public GameObject bulletPrefab; // �ӵ���Prefab
    public Transform firePoint; // �����ӵ���λ��
    public float fireRate = 1f; // �ӵ��ķ���Ƶ�ʣ�ÿ�뷢�伸�Σ�
    public float bulletSpeed = 10f; // �ӵ����ٶ�
    //private float nextFireTime = 0f;

    [Header("���ε�λ��")]
    public Transform[] points;

    //���μ�ʱ��
    private float mTimer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mTimer = 2f;
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //��ʼ��Ѫ����Ѫ��ui
        currentHealth = maxHealth;
        //��ʼ��׷��Ŀ��
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
    //��һ�׶�AI
    public void FirstBehaviour()
    {
        //������Һ�ʼ���С��Χ�ƶ�
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


    //���׶ι�����ʽ
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
        // �����ӵ�
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // �����ӵ��ķ�����ٶ�
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 direction = (player.position - firePoint.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }
    #endregion
    public void GetPlayerDistance()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
    }

    #region ��������
    public void TakeDamage(float damage)
    {
        currentHealth -= (damage + PlayeManager.instance.perDamage);
        //healthSlider.value = currentHealth;

        //OnHurt?.Invoke();//���˻ص�
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
