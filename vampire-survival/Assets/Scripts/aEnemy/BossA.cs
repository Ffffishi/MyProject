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
    public float moveSpeed;       // �ƶ��ٶ�

    //����Ч��
    public GameObject deathEffect;

    private float timer;
    [HideInInspector] public AIPath aiPath;
    [HideInInspector] public AIDestinationSetter destinationSetter;
    [HideInInspector] public float distanceToPlayer;
    public int expToGive;

    [Header("����")]
    [SerializeField] public float maxHealth;
    [HideInInspector] public float currentHealth;

    public Slider healthSlider;

    private Transform player;//���λ��
    private Transform randomTarget;//����ƶ�Ŀ��
    public float attackDe;//�������
    private float attackTimer;//������ʱ��
    private Transform attackTarget;//����Ԥ����ĵص�

    //public Transform pointRightUp, pointLeftDown;
    [Header("�����ĵ���")]
    public GameObject[] enemyPrefabs;
    [Header("���ε�λ��")]
    public Transform[] points;
    [Header("�ӵ�")]
    public GameObject bulletPrefab;

    public float bulletSpeed;  // �ӵ����ٶ�
    public int bulletCount; // �ӵ�������
    float angleStep; // ÿ���ӵ�֮��ĽǶȼ��
    float angle; // ��ʼ�Ƕ�
    //���μ�ʱ��
    private float mTimer;
    void Start()
    {
        angleStep = 360f / bulletCount; // ÿ���ӵ�֮��ĽǶȼ��
        angle = 0f; // ��ʼ�Ƕ�

        rb = GetComponent<Rigidbody2D>();
        mTimer = 2f;
        aiPath =GetComponent<AIPath>();
        destinationSetter=GetComponent<AIDestinationSetter>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //��ʼ��Ѫ����Ѫ��ui
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        //��ʼ��׷��Ŀ��
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
    //��һ�׶�AI
    public void FirstBehaviour()
    {
        //������Һ�ʼ���С��Χ�ƶ�
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
    //һ�׶ι�����ʽ
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

    //���׶ι�����ʽ
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

    #region ��������
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
