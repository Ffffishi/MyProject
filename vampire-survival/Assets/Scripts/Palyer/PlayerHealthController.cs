using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public float maxHealth ,currentHealth;
    public static PlayerHealthController instance;
    public Slider healthSlider;
    public GameObject deathEffect;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        maxHealth = PlayerStatesController.instance.health[0].value;
        currentHealth=maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        healthSlider.value = currentHealth;
    }
    private void Die()
    {
        gameObject.SetActive(false);
        LevelManager.instance.EndLevel();
        Instantiate(deathEffect, transform.position, transform.rotation);
    }
}
