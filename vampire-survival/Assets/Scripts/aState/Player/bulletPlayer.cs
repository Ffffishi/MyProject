using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletPlayer : MonoBehaviour
{
    public float lifeTime=4f; // 子弹存在的时间

    void Start()
    {
        Destroy(gameObject, lifeTime); // 一段时间后销毁子弹

    }
    private void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 可以在这里处理子弹击中玩家或其他物体后的逻辑
        // 例如，减少玩家生命值或销毁子弹
        if (hitInfo.CompareTag("Enemy"))
        {
            Destroy(this.gameObject); // 子弹击中玩家后销毁
        }
        
    }
}
