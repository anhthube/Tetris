using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        // Cập nhật UI health bar
        UpdateHealthBar();
    }

    void Die()
    {
        // Xử lý khi player chết
        Debug.Log("Player died!");
        Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        // Cập nhật UI health bar
        // Implement theo UI của bạn
    }
}
