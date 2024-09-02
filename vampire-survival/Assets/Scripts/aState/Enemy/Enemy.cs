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
    public float birthTime = 0f; // ���˳�����ʱ��
    [HideInInspector]
    public Transform player;
    public Transform targetPoint;

    [Header("���侭��")]
    public int expToGive;
    [Header("�˺�")]
    public float damage = 10f;
    [HideInInspector]
    public AIDestinationSetter destinationSetter;
    [HideInInspector]
    public AIPath aiPath;
    public bool isBirth=true;

    
    [Header("����")]
    //public bool isAttack;
    public LayerMask playerLayer;
    [HideInInspector] public float distanceToPlayer;
    public float attackRange;
    [Header("���˹���״̬")]
    [HideInInspector] public float detectionRange = 5f; // ���˼����ҵķ�Χ
    [HideInInspector] public float dashSpeed = 10f; // ����ٶ�
    [HideInInspector] public float dashDistance = 5f; // ��̾���
    [HideInInspector] public float cooldownTime = 2f; // �����ȴʱ��

    [HideInInspector] private Vector2 targetPosition;
    [HideInInspector] private Vector2 dashDirection;
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public float cooldownTimer = 0f;

    //�����������
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider2D enemyCollider;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    [HideInInspector] public float spawnDuration = 1f;
    private Vector3 originalScale;


    public UnityEvent OnHurt;
    public UnityEvent OnDie;
    [Header("���˻���")]
    [HideInInspector] public bool isHurt;
    public bool canKnockback;
    public float knockbackForce;
    public float knockbackTime;
    [HideInInspector]
    public float enemythirdTimer = 1f;

    //��ǰ״̬
    [HideInInspector] public IState currentState;
    //ʹ���ֵ�ӳ�䵽״̬��
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

        // ��ʼ��״̬��
        states.Add(EnemyState.Chase, new ChaseState(this));
        states.Add(EnemyState.Attack, new AttackState(this));
        states.Add(EnemyState.Hurt, new HurtState(this));
        states.Add(EnemyState.Dead, new DeathState(this));
        /*---------------*/
        //knockBackCounter = knockbackTime;
        // ���ó�ʼ״̬
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        // ��ʼ���ɶ���
        StartCoroutine(SpawnEffect());
        // ���ó�ʼ״̬��
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
    /// �л�״̬
    /// </summary>
    public void TransitionToState(EnemyState State)
    {
        // �˳���ǰ״̬
        if(currentState != null)
        {
            currentState.OnExitState();
        }
        // �л�״̬
        currentState = states[State];
        currentState.OnEnterState();
        
    }

    /// <summary>
    /// ��ȡ��Ҿ���
    /// </summary>
   public void GetPlayerDistance()
    {
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);

        //if (colliders.Length > 0)//����ڹ�����Χ��
        //{
        //    Debug.Log("����ڹ�����Χ��");
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
        OnHurt?.Invoke();//���˻ص�
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

    #region �����ű�
    public virtual void Attack()
    {
        //����ȴ���򲻽��й���
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
                //���߾���С�ڼ�ⷶΧ��ʼ׼�����
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
        dashDirection = (player.position - transform.position).normalized; // ���㳯����ҵķ���
        targetPosition = (Vector2)player.position + dashDirection;//��ȡ��ҵ�ǰλ�ò����϶������
        isDashing = true;//��ʼ���
    }

    void Dash()
    {
        //Ŀ��ص�
        transform.position += (Vector3)dashDirection * dashSpeed * Time.deltaTime * 0.8f;

        if (Vector2.Distance(transform.position, targetPosition) <= 0.1f ||
            Vector2.Distance(transform.position, player.position) >= dashDistance)
        {
            //�������Ŀ��ص�С��0.1��ֹͣ���
            isDashing = false;
            cooldownTimer = cooldownTime; // ��ʼ��ȴ
        }
    }
#endregion
    #region ���ɶ���Ч��
    /// <summary>
    /// ���ɶ���Ч��
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < spawnDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / spawnDuration;

            // �𽥷Ŵ����
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);

            // ������͸����
            spriteRenderer.color = new Color(1f, 1f, 1f, progress);

            yield return null;
        }

        // ȷ������״̬
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
