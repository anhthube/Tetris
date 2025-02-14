using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class playerControllerkillzombie : MonoBehaviour

{
    [Header("Player Settings")]
    public float detectionRange = 5f;  // Phạm vi phát hiện zombie

    [Header("Attack Settings")]
    [SerializeField] private float fireRate = 2f;      // Tốc độ bắn (giây)
    [SerializeField] private float bulletSpeed = 5f;     // Tốc độ đạn
    [SerializeField] private float bulletDamage = 20f;    // Sát thương mỗi viên đạn
    

    private int level = 1;
    private int gold = 0;
    private int upgradecost ;


    public GameObject bulletPrefab;     // Prefab của đạn
    public int coin = 0;
    private float nextFireTime = 0f;
    public static playerControllerkillzombie Instance;
    public TextMeshProUGUI cointext; // Ui coin ben ngoai
    public Text cointextUpdate; //coin update trong UIhero
    public TextMeshProUGUI upgradeMessage; // ui thong bao nang cap 
    public Text upgradeCoinText;
    public Text LevelHero;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        AutoShoot();
        updateUI();
    }

    public float getlevel() { return level; }
    public float getfireRate() { return fireRate; }
    public float getbulletSpeed() { return bulletSpeed; }
    public float getbulletDamage() { return bulletDamage; }
    void AutoShoot()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject nearestZombie = FindNearestZombie();

            if (nearestZombie != null)
            {
                // Bắn đạn
                Shoot(nearestZombie.transform.position);
                nextFireTime = Time.time + fireRate;
            }
        }
    }
    

    void updateUI()
    {
        if (cointext != null)
        {
            cointext.text = $": {coin}"; // Ui coin o ngoai
        }
        if (cointextUpdate != null)
        {
            cointextUpdate.text = $": {coin}"; //  ui coin nang cap o trong heroUi
        }

        if(upgradeCoinText != null) // hien thi vang can de nang cap hero
        {
             upgradecost = GetUpgradeCost();
            upgradeCoinText.text = $"{upgradecost}";
        }
        if(LevelHero != null)
        {
            LevelHero.text = $"level :{level}";    
        }
    }


    public void AddGold(int amount)
    {
        coin += amount;
       
    }

    GameObject FindNearestZombie()
    {
        // Tìm tất cả collider trong phạm vi detectionRange
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);

        // Lọc ra zombie gần nhất
        GameObject nearestZombie = colliders
            .Where(col => col.CompareTag("Zombie"))
            .OrderBy(col => Vector2.Distance(transform.position, col.transform.position))
            .Select(col => col.gameObject)
            .FirstOrDefault();

        return nearestZombie;
    }

    void Shoot(Vector2 targetPosition)
    {
        // Tạo đạn tại vị trí player
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Tính hướng đạn
        Vector2 direction = ((Vector2)targetPosition - (Vector2)transform.position).normalized;

        // Nếu bullet có Rigidbody2D
        if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = direction * bulletSpeed;
        }

        // Thêm component BulletBehavior vào đạn
        BulletBehavior bulletScript = bullet.GetComponent<BulletBehavior>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(bulletDamage); // Truyền damage từ player vào đạn
        }
    // Tự hủy đạn sau 2 giây
    Destroy(bullet, 2f);
    }
    public int GetUpgradeCost()
    {
        return level * 1;// Ví dụ: Giá nâng cấp = Cấp * 50 vàng
    }
    public void upgrade()
    {
        upgradecost = GetUpgradeCost();
        if (coin >=  upgradecost)
        {
            coin -= upgradecost;
            level++;

            fireRate *= 0.9f;
            bulletSpeed *= 1.1f;
            bulletDamage *= 1.5f;
           
            ShowUpgradeMessage("Nâng cấp thành công!", Color.green);
        }
        else
        {
            ShowUpgradeMessage("Không đủ vàng!", Color.red);
        }
    }
    void ShowUpgradeMessage(string message, Color color)
    {
        upgradeMessage.text = message;
        upgradeMessage.color = color;
        upgradeMessage.gameObject.SetActive(true);

        StopAllCoroutines(); 
        StartCoroutine(HideMessageAfterDelay());
    }
    IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        HideUpgradeMessage();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
    void HideUpgradeMessage()
    {
        
        upgradeMessage.gameObject.SetActive(false);
    }
}

    