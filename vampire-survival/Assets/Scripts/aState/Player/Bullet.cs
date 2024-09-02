using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime; // �ӵ����ڵ�ʱ��

    public int damage; // �ӵ���ɵ��˺�

    public string hurtObject; // ���ж�������
    void Start()
    {
        Destroy(gameObject, lifeTime); // һ��ʱ��������ӵ�
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // ���������ﴦ���ӵ�������һ������������߼�
        // ���磬�����������ֵ�������ӵ�
        if (hitInfo.CompareTag("Player")){
            PlayerHealthMgr.Instance.TakeDamage(damage);
            Destroy(this.gameObject); // �ӵ�������Һ�����
            //Debug.Log("Bullet hit player");
        }
        //Destroy(gameObject); // �ӵ�����Ŀ�������
    }
}
