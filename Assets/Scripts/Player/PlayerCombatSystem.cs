using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerCombatSystem : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private float attackRate = 2f;
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private StaminaBar staminaBar;
    [SerializeField] private int combo;
    [SerializeField] private bool attacking;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sounds;
    
    
    private const float staminaIncreasePerFrame = 5.0f;
    private float currentStamina;
    private float attackStamina = 20;
    private float nextAttackTime;
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }
    private void Update()
    {
        attackDamage = 25;
        if (Time.time >= nextAttackTime && currentStamina > 25)
        {
            if (Input.GetKeyDown(KeyCode.F) && !attacking)
            {
                if (combo == 1)
                {
                    attackDamage = 50;
                }
                Attack();
                currentStamina -= attackStamina;
                staminaBar.SetStamina(currentStamina);
                attacking = true;
                animator.SetTrigger(""+combo);
                nextAttackTime = Time.time + 1f / attackRate;
                audioSource.clip = sounds[combo];
                audioSource.Play();
                Debug.Log(attackDamage);
            }
        }
        
        currentStamina = Mathf.Clamp(currentStamina + (staminaIncreasePerFrame * Time.deltaTime), 0f, maxStamina);
        staminaBar.SetStamina(currentStamina);
    }
    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    private void OnDrawGizmos()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
    public void StartCombo()
    {
        attacking = false;
        if (combo < 2)
        {
            combo++; 
        }
    }

    public void FinishAnimation()
    {
        attacking = false;
        combo = 0;
    }
}
