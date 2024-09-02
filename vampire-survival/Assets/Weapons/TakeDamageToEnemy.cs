using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageToEnemy : MonoBehaviour
{
    public float damage = 1f;
    public float lifeTime, growSpeed = 5f;
    private Vector3 targetSize;
    public bool isShouldKnockBack = true;
    public bool isShouldDestroy;

    public bool damageOverTime;
    public float timeBetweenDamage;
    private float damageCounter;

    private List<EnemyController> enemyInRange=new List<EnemyController>();

    public bool destroyOnImpact;
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, lifeTime);
        targetSize=transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed*Time.deltaTime);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            targetSize = Vector3.zero;
            if(transform.localScale.x == 0f)
            {
                Destroy(gameObject);
                if (isShouldDestroy)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }

        if (damageOverTime == true)
        {
            damageCounter -= Time.deltaTime;
            if (damageCounter <= 0)
            {
                damageCounter = timeBetweenDamage;
                for (int i=0; i<enemyInRange.Count; i++)
                {
                    if (enemyInRange[i] != null)
                    {
                        enemyInRange[i].TakeDamage(damage, isShouldKnockBack);
                    }
                    else
                    {
                        enemyInRange.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (damageOverTime == false)
        {
            if (other.tag == "Enemy")
            {
                other.GetComponent<EnemyController>().TakeDamage(damage, isShouldKnockBack);
                if (destroyOnImpact)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (other.tag == "Enemy")
            {
                enemyInRange.Add(other.GetComponent<EnemyController>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (damageOverTime == true)
        {
            if (other.tag == "Enemy")
            {
                enemyInRange.Remove(other.GetComponent<EnemyController>());
            }
        }
    
    }
}
