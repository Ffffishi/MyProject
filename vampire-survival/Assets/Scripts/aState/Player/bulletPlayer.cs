using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletPlayer : MonoBehaviour
{
    public float lifeTime=4f; // �ӵ����ڵ�ʱ��

    void Start()
    {
        Destroy(gameObject, lifeTime); // һ��ʱ��������ӵ�

    }
    private void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // ���������ﴦ���ӵ�������һ������������߼�
        // ���磬�����������ֵ�������ӵ�
        if (hitInfo.CompareTag("Enemy"))
        {
            Destroy(this.gameObject); // �ӵ�������Һ�����
        }
        
    }
}
