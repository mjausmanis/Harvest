using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] float moveSpeed;
    [SerializeField] float minDistance;
    [SerializeField] HealthBar healthBar;
    [SerializeField] float attackRate;
    
    NavMeshAgent agent;
    public Animator animator;
    public int currentHealth = 0;

    float damageBuffer;
    bool attacking;
    bool isDead;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) {
            ChaseTarget();
        }
    }


    void ChaseTarget() {

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > minDistance) {
            agent.destination = player.transform.position;
            animator.SetBool("Attack", false);
        }

        if (distance <= minDistance && !attacking) {
            StartCoroutine(AttackPlayer());
        }
    }

    public void CheckHealth() {
        if(currentHealth <= 0 && !isDead) {
            Debug.Log("Enemy dead");
            Die();
        } else if (currentHealth > 0) {
            animator.SetBool("IsAlive", true);
        }
    }

    IEnumerator AttackPlayer() {
        attacking = true;
        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(attackRate);

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= minDistance) {
            player.gameObject.SendMessageUpwards("TakeDamage", 5);
        }
        attacking = false;
    }

    void ThrowApple() {

    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Get hit for " + damage);
        CheckHealth();
    }

    public void Die() {
        agent.isStopped = true;
        animator.SetBool("Attack", false);
        animator.SetBool("IsAlive", false);
        isDead = true;
    }
}
