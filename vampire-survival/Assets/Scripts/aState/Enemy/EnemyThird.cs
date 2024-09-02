using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThird : Enemy
{
    [Header("���ε�λ��")]
    public Transform[] points;
    private Transform randomTarget;//����ƶ�Ŀ��
    [Header("������ʽ")]
    public GameObject bulletPrefab; // �ӵ���Prefab
    public Transform firePoint; // �����ӵ���λ��
    public float fireRate = 1f; // �ӵ��ķ���Ƶ�ʣ�ÿ�뷢�伸�Σ�
    public float bulletSpeed = 10f; // �ӵ����ٶ�
    private float nextFireTime = 0f;
    //private float mTimer=1f;

    //���һ�����������͵ĵ��ˣ�������ʽΪ����Զ���ӵ�

    //private void FixedUpdate()
    //{
    //    //base.FixedUpdate();
    //    //if (currentHealth <= 0)
    //    //{
    //    //    Destroy(gameObject);
    //    //}
    //    //birthTime += Time.fixedDeltaTime; // ��¼���˳�����ʱ��
    //}
    public override void Attack()
    {
        if (aiPath.enabled == false)
            aiPath.enabled = true;
        //aiPath.canMove = false; // ֹͣ�ƶ�
        if (cooldownTimer > 0)
        {
            //    Debug.Log("��ȴ��" + cooldownTimer);
            cooldownTimer -= Time.deltaTime;
            TransitionToState(EnemyState.Chase);
            return;
        }
        //Debug.Log("attack");

        // �����ǰʱ������´η���ʱ��
        if (Time.time >= nextFireTime&&birthTime>=1f)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate; // �����´η����ʱ��
            cooldownTimer = cooldownTime; // ��ʼ��ȴ
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

    public override void StopChase()
    {
        //mTimer -= Time.deltaTime;
        if (enemythirdTimer <= 0)
        {
            randomTarget = points[Random.Range(0, points.Length)];
            enemythirdTimer = 1f;
        }
        //aiPath.enabled = !(aiPath.enabled); // ֹͣ׷��
        //randomTarget = points[Random.Range(0, points.Length)];
        destinationSetter.target=randomTarget;
    }
}
