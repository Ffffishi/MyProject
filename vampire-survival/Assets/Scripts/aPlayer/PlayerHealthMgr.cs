using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthMgr : Character 
{
    private static PlayerHealthMgr instance;
    //private bool isNone = false;
    public static PlayerHealthMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerHealthMgr>();
            }
            return instance;
        }
    }

    [Header("ÎÞµÐÊ±¼ä")]
    public bool isInvincible;

    public float invincibleTime = 2.0f;

    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            return;
        }
        currentHealth -= damage;
        StartCoroutine(Invincible());
        //Debug.Log("PlayerHealth: " + currentHealth);
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            UIController.instance.ShowDes();
            UIController.instance.levelEndPanel.SetActive(true);
            Time.timeScale = 0f;
            //Invoke("ShowEndLevelPanel", 2f);
            Die();
            
        }
    }

    public IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}
