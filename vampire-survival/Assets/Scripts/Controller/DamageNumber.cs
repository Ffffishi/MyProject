using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageNumber : MonoBehaviour
{
    public TMP_Text damageText;
    public float lifeTime;
    private float lifeCounter;
    private float floatSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        lifeCounter = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeCounter > 0)
        {
            lifeCounter -= Time.deltaTime;
            //damageText.enabled = true;
            if (lifeCounter <= 0)
            {
                //Destroy(gameObject);
                DNController.instance.ReturnToPool(this);
            }
        }
        transform.position+=Vector3.up*floatSpeed*Time.deltaTime;

    }

    public void SetUp(int damageDisplay)
    {
        lifeCounter = lifeTime;
        damageText.text = damageDisplay.ToString();
        damageText.enabled = true;
    }
}
