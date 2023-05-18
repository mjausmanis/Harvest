using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health;
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void CheckHealth() {
        if(health <= 0 && !isDead) {
            Debug.Log("Enemy dead");
            Die();
        }
    }

    public void GetHit(float damage) {
        health -= damage;
        CheckHealth();
    }

    public void Die() {
        transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
        isDead = true;
    }
}
