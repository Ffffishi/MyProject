using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sword : WeaponsF
{
    public EnemyDamage damager;
    public GameObject sword;

    public Rigidbody2D rb;
    public float weaponRange;
    public float moveSpeed;
    public LayerMask whatIsEnemy;

    private Vector3 startPosition;
    public Transform offset;
    private float attackTimer;
    private bool isAttack;

    //private int state;
    void Start()
    {
        //offset = new Vector3(-1, 0, 0);
        attackTimer = 0f;
        SetStat();
        isAttack = true;
        //state = 0;
    }

    private void Update()
    {
        attackTimer-=Time.deltaTime;
        if (attackTimer <= 0)
        {
            FindEnemy();
            isAttack = true;
            attackTimer = 1f;
            if (Vector2.Distance(sword.transform.position, offset.position) > weaponRange)
            {
                Vector2 direction = (offset.position - sword.transform.position).normalized;
                rb.velocity=direction*moveSpeed;
                isAttack = false;
            }
        }
        if (Vector2.Distance(sword.transform.position, offset.position) > weaponRange)
        {
            Vector2 direction = (offset.position - sword.transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            isAttack = false;
        }
        if (Vector2.Distance(sword.transform.position, offset.position) < 0.1f && isAttack == false)
        {
            rb.velocity = Vector2.zero;
            Follow();
        }
        if (isStatsUpdated == true)
        {
            isStatsUpdated = false;
            SetStat();
        }
    }

    private void FixedUpdate()
    {

    }

    public void FindEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange, whatIsEnemy);
        if (enemies.Length > 0)
        {
            Vector3 targetPos = enemies[Random.Range(0, enemies.Length)].transform.position;
            Vector2 direction = (targetPos - sword.transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }
    public void Follow()
    {
        sword.transform.position = Vector3.MoveTowards(sword.transform.position, offset.position, moveSpeed * Time.deltaTime); // 跟随offset的位置
        
    }

    


    /// <summary>
    /// 设置武器的属性
    /// </summary>
    public void SetStat()
    {
        damager.damage = stats[weaponLevel].damage;
    }
}
