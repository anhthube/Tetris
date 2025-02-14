using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public GameObject damagePopupPrefab;
    public float damage ; // Ví dụ damage là 20


    public void Initialize(float playerDamage)
    {
        damage = playerDamage;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Zombie"))
        {
            if (damagePopupPrefab != null)
            {
                GameObject damagePopupObject = Instantiate(damagePopupPrefab,
                    other.transform.position + Vector3.up * 0.5f,
                    Quaternion.identity);
                DamagePopup damagePopup = damagePopupObject.GetComponent<DamagePopup>();
                damagePopup.Setup(damage);
            }

            // Gọi hàm nhận damage của zombie
            ZombieHealth zombieHealth = other.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
