using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] public int MaxPlayerHealth = 100;
    
    public int currentHealth = 0;

    private bool dead = false;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxPlayerHealth;
        healthBar.SetMaxHealth(MaxPlayerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TakeDamage(int damage) {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Player Health: " + currentHealth);
        CheckHealth();
    }

    private void CheckHealth() {
        if (currentHealth <= 0) {
            Debug.Log("Player dead");
            dead = true;
        }
    }

    private void Heal(int hp) {
        currentHealth += hp;
        if (currentHealth > MaxPlayerHealth) {
            currentHealth = MaxPlayerHealth;
        }
        healthBar.SetHealth(currentHealth);
    }
}
