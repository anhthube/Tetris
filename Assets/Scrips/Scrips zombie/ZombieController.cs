using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    
    [Header("Zombie attack")]
    public float moveSpeed = 10f;
    public float attackRange = 1f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    private Transform player;
    private float nextAttackTime = 0f;
    private Rigidbody2D rb;
    private SpamZombies waveManager;
    void Start()
    {
        // Tìm player bằng tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (player == null) return;

        // Tính khoảng cách đến player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Nếu trong tầm đánh
        if (distanceToPlayer <= attackRange)
        {
            // Kiểm tra cooldown
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            // Di chuyển về phía player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
        }
    }
   

    public void Initialize(SpamZombies manager)
    {
        waveManager = manager;
    }

    void OnDestroy()
    {
        if (waveManager != null)
        {
            waveManager.OnZombieKilled();
        }
    }

    void Attack()
    {
        // Gọi hàm nhận damage của player
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Vẽ range attack trong Editor để dễ nhìn
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
   
}
