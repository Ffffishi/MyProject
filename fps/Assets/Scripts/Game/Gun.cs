using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

//ǹ�Ľű�
public class Gun : MonoBehaviour
{

    public int BulletCount;

    public GameObject bulletPrefab;
    public GameObject casingPreafab;

    public Transform bulletTf;
    public Transform casingTf;

    void Start()
    {
        if (Game.isFixed == false)
        {
            BulletCount = 10;
        }
        else
        {
            BulletCount = 5;
        }
    }

    public void Attack()
{
    // �����ӵ�
    if (bulletPrefab != null && bulletTf != null)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, bulletTf.position, bulletTf.rotation);
        if (bulletObj != null)
        {
            //bulletObj.transform.position = bulletTf.transform.position;
            Rigidbody bulletRb = bulletObj.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.AddForce(transform.forward * 500, ForceMode.Impulse); // �ӵ��ٶ�
            }
        }
    }

        //���ɵ���
    if (casingPreafab != null && casingTf != null)
        {
            GameObject casingObj = Instantiate(casingPreafab);
            if (casingObj != null)
            {
                casingObj.transform.position = casingTf.transform.position;
            }
        }
    }



}