using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    
    [SerializeField] public int MaxPlayerHealth = 100;
    
    public int currentHealth = 0;


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
        
        CheckHealth();
    }

    private void CheckHealth() {
        if (currentHealth <= 0) {
            StartCoroutine(Die());
        }
    }

    private void Heal(int hp) {
        currentHealth += hp;
        if (currentHealth > MaxPlayerHealth) {
            currentHealth = MaxPlayerHealth;
        }
        healthBar.SetHealth(currentHealth);
    }

    private IEnumerator Die() {
        Transform gameOverlay = transform.Find("PlayerHUD/GameOver");

        gameOverlay.gameObject.SetActive(true);
        Time.timeScale = 0.3f;

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
