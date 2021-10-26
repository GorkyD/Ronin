using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private HealthBar healthBar;
    
    private Animator animator;
    private int currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(TakeDamageAnimation(damage));
        
    }
    IEnumerator TakeDamageAnimation(int damage)
    {
        yield return new WaitForSeconds(0.8f);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        animator.SetTrigger("Hurt");
        
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        enemy.GetComponent<EnemyCombatSystem>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(enemyHealthBar);
        animator.SetTrigger("Death");
        StopAllCoroutines();
        yield return new WaitForSeconds(10f);
        Destroy(enemy);
    }

}
