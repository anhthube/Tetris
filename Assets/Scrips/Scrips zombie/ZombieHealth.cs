using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;


    public GameObject Coinprefab;
    public float dropcoi = 0.5f;
    public int minGold = 1;
    public int maxGold = 5;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            if (Random.value <= dropcoi)
            {
                dropCoin();
            }
            Die();
        }
    }

    void Die()
    {
        // Xử lý khi zombie chết
        Destroy(gameObject);
    }
    private void dropCoin()
    {
        if (Coinprefab != null)
        {
            int goldAmount = Random.Range(minGold, maxGold + 1);

            GameObject goldcoin = Instantiate(Coinprefab, transform.position, Quaternion.identity);

            GoldCoin coin = goldcoin.GetComponent<GoldCoin>();
            if (coin != null)
            {
                coin.value = goldAmount;
            }

        }
    }
}
