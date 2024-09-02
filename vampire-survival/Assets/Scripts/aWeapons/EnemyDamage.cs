using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damage;

    public bool canKnockback;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if(other.GetComponent<BossA>() != null){
                other.GetComponent<BossA>().TakeDamage(damage);
                return;
            }
            if (other.GetComponent<EPlus>() != null)
            {
                other.GetComponent<EPlus>().TakeDamage(damage);
                return;
            }
            if (other.GetComponent<Enemy>() == null)
            {
                other.GetComponent<EnemyMgr>().TakeDamage(damage, canKnockback);
            }
            else
            {
                other.GetComponent<Enemy>().TakeDamage(damage, canKnockback);
            }
        }
    }

}
