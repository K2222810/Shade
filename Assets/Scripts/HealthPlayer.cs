using System;
using System.Collections;
using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public HealthUI healthUI;

    private SpriteRenderer spriteRenderer;

    public static event Action OnPlayDied; 

    // Start is called before the first frame update
    void Start()
    {
        ResetHealth(); 

        spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.OnReset += ResetHealth; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth); 
    
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);


        StartCoroutine(FlashRed()); 
        //Flash Red

        if (currentHealth <= 0)
        {
            OnPlayDied.Invoke();    
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
