using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] int maxHealth;
    [SerializeField] float moveSpeed;
    [SerializeField] float minDistance;
    [SerializeField] HealthBar healthBar;
    [SerializeField] float attackRate;

    [Header("Grenade")]
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] int nadeCount;
    [SerializeField] float throwForce = 10f;
    
    NavMeshAgent agent;
    Animator animator;
    int currentHealth = 0;

    float damageBuffer;
    bool attacking;
    bool isDead;
    GameObject player;
    bool readyToThrow = true;

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

    void Update()
    {
        if (!isDead) {
            ChaseTarget();
        }
    }

    void ChaseTarget() {

        float distance = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log(distance);
        if (distance > minDistance) {
            agent.destination = player.transform.position;
            animator.SetBool("Attack", false);
        }

        if (nadeCount > 0 && readyToThrow && distance > 20 && distance < 25) {
            StartCoroutine(ThrowNade());
        }

        if (distance <= minDistance && !attacking) {
            StartCoroutine(AttackPlayer());
        }
    }

    public void CheckHealth() {
        if(currentHealth <= 0 && !isDead) {
            Die();
        } else if (currentHealth > 0) {
            animator.SetBool("IsAlive", true);
        }
    }

    IEnumerator AttackPlayer() {
        attacking = true;
        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(0.5f);

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= minDistance) {
            player.gameObject.SendMessageUpwards("TakeDamage", 5);
        }
        
        yield return new WaitForSeconds(0.5f);

        attacking = false;
    }

    IEnumerator ThrowNade() {
        readyToThrow = false;
        nadeCount --;
        animator.Play("Throw Nade");
        yield return new WaitForSeconds(0.5f);

        GameObject grenade = Instantiate(grenadePrefab, transform.position + new Vector3(0f, 3f, 0f), Quaternion.identity);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        float curveHeight = 2f;
        Vector3 throwVector = Quaternion.Euler(curveHeight, 0f, 0f) * direction;
        Rigidbody grenadeRigidbody = grenade.GetComponent<Rigidbody>();
        grenadeRigidbody.velocity = throwVector * throwForce;

        yield return new WaitForSeconds(3);

        animator.ResetTrigger("ThrowNade");
        readyToThrow = true;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Get hit for " + damage);
        CheckHealth();
    }

    public void Die() {
        animator.SetBool("IsAlive", false);
        animator.SetBool("Attack", false);
        agent.isStopped = true;
        isDead = true;
    }

    public bool checkIfDead() {
        return isDead;
    }
}
