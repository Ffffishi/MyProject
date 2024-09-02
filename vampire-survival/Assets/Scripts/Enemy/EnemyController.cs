using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    //public float maxHealth = 100;
    public float currentHealth;
    public float knockBackTime=.5f;
    private float knockBackCounter;
    private Transform player;
    private float damage = 20;
    private float hitWaitTime=1f, hitTimer;

    private AIDestinationSetter destinationSetter;


    public int expToGive=1;

    public int coinValue=1;
    public float coinDropRate=.5f;
    void Init()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
        player = PlayerHealthController.instance.transform;
        Init();
        //currentHealth = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.gameObject.activeSelf == true)
        {
            rb.velocity = (player.position - transform.position).normalized * speed;
            if (knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;
                if (speed > 0)
                {
                    speed = -speed * 2f;
                }
                if (knockBackCounter <= 0)
                {
                    speed = Mathf.Abs(speed * .5f);
                }
                if (hitTimer > 0f)
                {
                    hitTimer -= Time.deltaTime;
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag=="Player"&&hitTimer<=0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);
            hitTimer=hitWaitTime;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            EXLevelController.instance.SpawnExp(transform.position,expToGive);

            if (Random.value <= coinDropRate)
            {
                CoinController.instance.DropCoin(transform.position, coinValue);
            }
        }

        DNController.instance.SpwanDamageNumber(transform.position, damage);
    }
    public void TakeDamage(float damage,bool isShouldKnockBack)
    {
        TakeDamage(damage);
        if (isShouldKnockBack)
        {
            //Vector2 knockBackDir = (player.position - transform.position).normalized;
            //rb.velocity = knockBackDir * speed;
            knockBackCounter = knockBackTime;
        }
    }
}
