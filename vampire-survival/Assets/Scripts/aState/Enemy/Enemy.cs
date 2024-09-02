using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyState
{
    Chase,
    Attack,
    Hurt,
    Dead
}

public class Enemy : Character
{
    [HideInInspector]
    public float birthTime = 0f; // 敌人出生的时间
    [HideInInspector]
    public Transform player;
    public Transform targetPoint;

    [Header("掉落经验")]
    public int expToGive;
    [Header("伤害")]
    public float damage = 10f;
    [HideInInspector]
    public AIDestinationSetter destinationSetter;
    [HideInInspector]
    public AIPath aiPath;
    public bool isBirth=true;

    
    [Header("攻击")]
    //public bool isAttack;
    public LayerMask playerLayer;
    [HideInInspector] public float distanceToPlayer;
    public float attackRange;
    [Header("敌人攻击状态")]
    [HideInInspector] public float detectionRange = 5f; // 敌人检测玩家的范围
    [HideInInspector] public float dashSpeed = 10f; // 冲刺速度
    [HideInInspector] public float dashDistance = 5f; // 冲刺距离
    [HideInInspector] public float cooldownTime = 2f; // 冲刺冷却时间

    [HideInInspector] private Vector2 targetPosition;
    [HideInInspector] private Vector2 dashDirection;
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public float cooldownTimer = 0f;

    //敌人自身组件
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider2D enemyCollider;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    [HideInInspector] public float spawnDuration = 1f;
    private Vector3 originalScale;


    public UnityEvent OnHurt;
    public UnityEvent OnDie;
    [Header("受伤击退")]
    [HideInInspector] public bool isHurt;
    public bool canKnockback;
    public float knockbackForce;
    public float knockbackTime;
    [HideInInspector]
    public float enemythirdTimer = 1f;

    //当前状态
    [HideInInspector] public IState currentState;
    //使用字典映射到状态机
    [HideInInspector] public Dictionary<EnemyState, IState> states = new Dictionary<EnemyState, IState>();
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aiPath = GetComponent<AIPath>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPoint = player;
        destinationSetter = GetComponent<AIDestinationSetter>();

        // 初始化状态机
        states.Add(EnemyState.Chase, new ChaseState(this));
        states.Add(EnemyState.Attack, new AttackState(this));
        states.Add(EnemyState.Hurt, new HurtState(this));
        states.Add(EnemyState.Dead, new DeathState(this));
        /*---------------*/
        //knockBackCounter = knockbackTime;
        // 设置初始状态
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        // 开始生成动画
        StartCoroutine(SpawnEffect());
        // 设置初始状态机
        TransitionToState(EnemyState.Chase);
    }

    void Update()
    {
        enemythirdTimer-=Time.deltaTime;
        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        currentState.OnUpdateState();
    }

    void FixedUpdate()
    {
        birthTime += Time.fixedDeltaTime;
        currentState.OnFixedUpdateState();
    }


    /// <summary>
    /// 切换状态
    /// </summary>
    public void TransitionToState(EnemyState State)
    {
        // 退出当前状态
        if(currentState != null)
        {
            currentState.OnExitState();
        }
        // 切换状态
        currentState = states[State];
        currentState.OnEnterState();
        
    }

    /// <summary>
    /// 获取玩家距离
    /// </summary>
   public void GetPlayerDistance()
    {
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);

        //if (colliders.Length > 0)//玩家在攻击范围内
        //{
        //    Debug.Log("玩家在攻击范围内");
        //    isAttack=true;
        //}
        //else
        //{
        //    isAttack = false;
        //}
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
    }
    
    public void EnemyHurt()
    {
        isHurt = true;
    }

    //void Update()
    //{
    //    if (knockBackCounter > 0)
    //    {
    //        knockBackCounter -= Time.deltaTime;
    //        if (aiPath.maxSpeed > 0)
    //        {
    //            aiPath.maxSpeed = -aiPath.maxSpeed * 2f;
    //        }
    //        if (knockBackCounter <= 0)
    //        {
    //            aiPath.maxSpeed = Mathf.Abs(aiPath.maxSpeed * .5f);
    //        }
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealthMgr.Instance.TakeDamage(damage);
        }
    }

    public virtual void StopChase()
    {

    }

    public override void TakeDamage(float damage, bool shouldKnockback)
    {
        currentHealth -= (damage + PlayeManager.instance.perDamage);
        OnHurt?.Invoke();//受伤回调
        if (currentHealth <= 0)
        {
            OnDie?.Invoke();
            Die();
            LevelMgr.instance.enemyCounter++;
            ExpController.instance.SpawnPickup(transform.position, expToGive);
            ExpController.instance.SpawnItems(transform.position);

        }
        DNController.instance.SpwanDamageNumber(transform.position, damage + PlayeManager.instance.perDamage);
    }

    #region 攻击脚本
    public virtual void Attack()
    {
        //在冷却中则不进行攻击
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            TransitionToState(EnemyState.Chase);
            return;
        }
        if (!isDashing&& transform.localScale == originalScale)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                //二者距离小于检测范围则开始准备冲刺
                StartDash();
            }
        }
        else
        {
            Dash();
        }
    }
    void StartDash()
    {
        dashDirection = (player.position - transform.position).normalized; // 计算朝向玩家的方向
        targetPosition = (Vector2)player.position + dashDirection;//获取玩家当前位置并加上额外距离
        isDashing = true;//开始冲刺
    }

    void Dash()
    {
        //目标地点
        transform.position += (Vector3)dashDirection * dashSpeed * Time.deltaTime * 0.8f;

        if (Vector2.Distance(transform.position, targetPosition) <= 0.1f ||
            Vector2.Distance(transform.position, player.position) >= dashDistance)
        {
            //如果距离目标地点小于0.1则停止冲刺
            isDashing = false;
            cooldownTimer = cooldownTime; // 开始冷却
        }
    }
#endregion
    #region 生成动画效果
    /// <summary>
    /// 生成动画效果
    /// </summary>
    /// <returns></returns>
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
    #endregion

    public void EnemyDie()
    {
        TransitionToState(EnemyState.Dead);
    }

    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
