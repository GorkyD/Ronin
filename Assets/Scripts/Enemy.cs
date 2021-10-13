using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private HealthBar healthBar;
    private Animator _animator;
    
    private int currentHealth;
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        _animator.SetTrigger("Hurt");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        _animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        Destroy(enemyHealthBar);

        StartCoroutine(Death());
    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(5f);
        deathEffect.transform.position =
            new Vector3(transform.position.x, transform.position.y+ 0.3f, transform.position.z + 2);
        yield return new WaitForSeconds(0.35f); 
        Destroy(enemy);
    }
}
