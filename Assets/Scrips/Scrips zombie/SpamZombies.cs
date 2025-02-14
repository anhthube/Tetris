using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpamZombies : MonoBehaviour
{
    [Header("Wave Settings")]
    public int currentWave = 1;
    public float waveTimeLimit = 30f;
    //public float timeBetweenWaves = 10f;
    public int baseZombiesPerWave = 3;
    public float waveZombieIncrease = 1.5f;

    [Header("Spawn Settings")]
    public GameObject[] zombiePrefabs;
    public float minSpawnInterval = 0.3f;
    public float maxSpawnInterval = 1f;
    public float spawnRadius = 1f;

    [Header("UI References")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI zombieCountText;
  

    // Private variables
    private int zombiesLeftToSpawn;
    private bool isWaveActive = false;
    private int activeZombies = 0;
    private float currentWaveTime = 0f;
    private bool isSpawning = false;
    private int totalZombiesKilled = 0; // Thêm biến đếm tổng số zombie đã tiêu diệt
    private Coroutine spawnCoroutine;
    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        currentWave = 1;
        totalZombiesKilled = 0;
        StartNewWave();
    }

    private void Update()
    {
        if (isWaveActive)
        {
            currentWaveTime += Time.deltaTime;
            UpdateUI();

            if (currentWaveTime >= waveTimeLimit)
            {
                StartCoroutine(EndWave());
            }

            // Kiểm tra điều kiện kết thúc wave
            if (zombiesLeftToSpawn <= 0 && activeZombies <= 0)
            {
                StartCoroutine(EndWave());
            }
        }
    }

    void UpdateUI()
    {
        if (waveText != null)
            waveText.text = $"Wave: {currentWave}";

        if (timerText != null)
        {
            float timeLeft = Mathf.Max(0, waveTimeLimit - currentWaveTime);
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }

        if (zombieCountText != null)
            zombieCountText.text = $"Zombies: {zombiesLeftToSpawn + activeZombies}";

       
    }

    void StartNewWave()
    {
        zombiesLeftToSpawn = CalculateWaveZombies();
        isWaveActive = true;
        currentWaveTime = 0f;
        UpdateUI();
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnWaveZombies());
    }

    int CalculateWaveZombies()
    {
        return Mathf.RoundToInt(baseZombiesPerWave * Mathf.Pow(waveZombieIncrease, currentWave - 1));
    }
    void OnDisable()
    {
        // Dừng coroutine khi object bị disable hoặc destroy
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    void OnDestroy()
    {
        // Cleanup khi object bị destroy
        StopAllCoroutines();
    }

    IEnumerator SpawnWaveZombies()
    {
        if (isSpawning) yield break;

        isSpawning = true;
        while (zombiesLeftToSpawn > 0 && currentWaveTime < waveTimeLimit)
        {
            SpawnZombie();
            zombiesLeftToSpawn--;
            activeZombies++; // Tăng số zombie active khi spawn

            float spawnDelay = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnDelay);
        }
        isSpawning = false;
    }

    void SpawnZombie()
    {
        float randomAngle = Random.Range(0f, 180f);
        Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.right;
        Vector2 spawnPosition = (Vector2)transform.position + direction * spawnRadius;

        GameObject zombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
        GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

        // Thêm component theo dõi zombie
        ZombieController zombieController = zombie.AddComponent<ZombieController>();
        zombieController.Initialize(this);

        if (!zombie.GetComponent<Rigidbody2D>())
        {
            Rigidbody2D rb = zombie.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }
    }

    // Thêm method để xử lý khi zombie bị tiêu diệt
    public void OnZombieKilled()
    {
        activeZombies--;
        totalZombiesKilled++;

        if (zombiesLeftToSpawn <= 0 && activeZombies <= 0)
        {
            StartCoroutine(EndWave());
        }
    }

    IEnumerator EndWave()
    {
        if (!isWaveActive) yield break;

        isWaveActive = false;
        isSpawning = false;

      

        //yield return new WaitForSeconds(timeBetweenWaves);

        currentWave++;
        StartNewWave();
    }

    // Getter methods
    public int GetCurrentWave() => currentWave;

    public int GetRemainingZombies() => zombiesLeftToSpawn;
    public int GetActiveZombies() => activeZombies;
    public int GetTotalKills() => totalZombiesKilled;
    public bool IsWaveActive() => isWaveActive;
}


