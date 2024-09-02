using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr : Character
{
    [Header("目标")]
    public Transform player;
    [Header("掉落经验")]
    public int expToGive;
    [Header("伤害")]
    public float damage = 10f;
    private AIDestinationSetter destinationSetter;
    private AIPath aiPath;

    //private SpriteRenderer spriteRenderer;

    public float spawnDuration = 1f;
    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;

    public float knockbackTime = 0.5f;
    private float knockBackCounter;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        aiPath = GetComponent<AIPath>();
        knockBackCounter=knockbackTime;
        // 设置初始状态
        transform.localScale = Vector3.zero;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        // 开始生成动画
        StartCoroutine(SpawnEffect());

        Init();
    }

    void Update()
    {
        if(player.position.x < transform.position.x){
            spriteRenderer.flipX = true;
        }else{
            spriteRenderer.flipX = false;
        }
        if (knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;
            if (aiPath.maxSpeed > 0)
            {
                aiPath.maxSpeed = -aiPath.maxSpeed * 2f;
            }
            if (knockBackCounter <= 0)
            {
                aiPath.maxSpeed = Mathf.Abs(aiPath.maxSpeed * .5f);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealthMgr.Instance.TakeDamage(damage);
        }
    }

    public override void TakeDamage(float damage,bool shouldKnockback)
    {
        currentHealth -= (damage+PlayeManager.instance.perDamage);
        if (currentHealth <= 0)
        {
            Die();
            ExpController.instance.SpawnPickup(transform.position, expToGive);
            LevelMgr.instance.enemyCounter++;
        }
        DNController.instance.SpwanDamageNumber(transform.position, damage + PlayeManager.instance.perDamage);
        if (shouldKnockback)
        {
            knockBackCounter = knockbackTime;
        }
    }


    void Init()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = player;
    }


    IEnumerator SpawnEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < spawnDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / spawnDuration;

            // 逐渐放大敌人
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);

            // 逐渐增加透明度
            spriteRenderer.color = new Color(1f, 1f, 1f, progress);

            yield return null;
        }

        // 确保最终状态
        transform.localScale = originalScale;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
