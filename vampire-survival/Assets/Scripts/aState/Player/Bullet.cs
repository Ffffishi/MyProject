using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime; // 子弹存在的时间

    public int damage; // 子弹造成的伤害

    public string hurtObject; // 击中对象名称
    void Start()
    {
        Destroy(gameObject, lifeTime); // 一段时间后销毁子弹
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 可以在这里处理子弹击中玩家或其他物体后的逻辑
        // 例如，减少玩家生命值或销毁子弹
        if (hitInfo.CompareTag("Player")){
            PlayerHealthMgr.Instance.TakeDamage(damage);
            Destroy(this.gameObject); // 子弹击中玩家后销毁
            //Debug.Log("Bullet hit player");
        }
        //Destroy(gameObject); // 子弹击中目标后销毁
    }
}
