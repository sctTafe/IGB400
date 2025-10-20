using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

public class EnemySpawner_Singleton : Singleton<EnemySpawner_Singleton>
{
    [Header("Enemy Prefabs")]
    [SerializeField] private List<GameObject> basicEnemyPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> bossPrefabs = new List<GameObject>();

    [Header("Spawn Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float basicEnemySpawnInterval = 5f;
    [SerializeField] private float bossSpawnInterval = 30f;
    [SerializeField] private float spawnHeightBasic = 10f;
    [SerializeField] private float spawnHeightBoss = 15f;
    [SerializeField] private float bossSpawnDelay = 30f;

    [Header("Health Scaling Settings")]
    [SerializeField] private int startingHealth = 10;
    [SerializeField] private int healthAtScaleDurationMax = 100;
    [SerializeField] private float scalingDuration = 360; // seconds
    [SerializeField] private int bossHealthMultiplier = 5; // Bosses have 5x basic enemy health

    private float nextBasicSpawnTime;
    private float nextBossSpawnTime;
    private bool delayingBasicSpawn = false;
    private float gameStartTime;
    private int permanentHealthBonus = 0; // Additional health added via public function
    bool isSpawningBosses;
    bool isSpawningBasic;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not set in EnemySpawner!");
            return;
        }
        if (basicEnemyPrefabs.Count == 0)
        {
            Debug.LogWarning("No basic enemy prefabs assigned to EnemySpawner!");
        }
        if (bossPrefabs.Count == 0)
        {
            Debug.LogWarning("No boss prefabs assigned to EnemySpawner!");
        }

        gameStartTime = Time.time;
        nextBasicSpawnTime = Time.time + basicEnemySpawnInterval;
        nextBossSpawnTime = Time.time + bossSpawnInterval;

        if (bossPrefabs.Count > 0)
            isSpawningBosses = true;
        if (basicEnemyPrefabs.Count > 0)
            isSpawningBasic = true;
    }

    void Update()
    {
        if (player == null)
            return;

        if (isSpawningBasic)
        {
            // Check for basic enemy spawn
            if (Time.time >= nextBasicSpawnTime)
            {
                SpawnBasicEnemy();
                
            }
        }

        if (isSpawningBosses)
        {
            // Check for boss spawn
            if (Time.time >= nextBossSpawnTime)
            {
                SpawnBoss();
                nextBossSpawnTime = Time.time + bossSpawnInterval;
            }
        }
    }

    /// <summary>
    /// Calculates the current health value based on game time
    /// </summary>
    private int CalculateCurrentBasicEnemyHealth()
    {
        float elapsedTime = Time.time - gameStartTime;

        // Calculate linear interpolation from starting health to health at 4 minutes
        float t = Mathf.Clamp01(elapsedTime / scalingDuration);
        int scaledHealth = Mathf.RoundToInt(Mathf.Lerp(startingHealth, healthAtScaleDurationMax, t));

        // Add permanent bonus from public function calls
        return scaledHealth + permanentHealthBonus;
    }

    private void SpawnBasicEnemy()
    {
        nextBasicSpawnTime = Time.time + basicEnemySpawnInterval;

        GameObject prefab = basicEnemyPrefabs[Random.Range(0, basicEnemyPrefabs.Count)];
        Vector3 spawnPos = new Vector3(player.position.x, player.position.y + spawnHeightBasic, player.position.z);

        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Get the ObjectHealth component and modify it
        ObjectHealth healthComponent = enemy.GetComponent<ObjectHealth>();
        if (healthComponent != null)
        {
            int enemyHealth = CalculateCurrentBasicEnemyHealth();
            healthComponent.fn_SetNewMaxAndCurrentHealth(enemyHealth);
        }
        else
        {
            Debug.LogWarning("No ObjectHealth component found on spawned basic enemy!");
        }
    }

    private void SpawnBoss()
    {
        GameObject prefab = bossPrefabs[Random.Range(0, bossPrefabs.Count)];
        Vector3 spawnPos = new Vector3(player.position.x, player.position.y + spawnHeightBoss, player.position.z);

        GameObject boss = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Get the ObjectHealth component and modify it
        ObjectHealth healthComponent = boss.GetComponent<ObjectHealth>();
        if (healthComponent != null)
        {
            // Bosses have health multiplied by the boss multiplier
            int bossHealth = CalculateCurrentBasicEnemyHealth() * bossHealthMultiplier;
            healthComponent.fn_SetNewMaxAndCurrentHealth(bossHealth);
        }
        else
        {
            Debug.LogWarning("No ObjectHealth component found on spawned boss!");
        }

        // Delay the next basic enemy spawn
        StartCoroutine(DelayBasicSpawn());
    }

    private IEnumerator DelayBasicSpawn()
    {
        delayingBasicSpawn = true;
        yield return new WaitForSeconds(bossSpawnDelay);
        nextBasicSpawnTime = Time.time + basicEnemySpawnInterval;
        delayingBasicSpawn = false;
    }

    /// <summary>
    /// Public function to immediately spawn a basic enemy and permanently increase base health by 5
    /// </summary>
    public void fn_SpawnEnemyAndIncreaseHealth()
    {
        if (basicEnemyPrefabs.Count == 0)
        {
            Debug.LogWarning("Cannot spawn enemy - no basic enemy prefabs assigned!");
            return;
        }

        // Increase permanent health bonus
        permanentHealthBonus += 5;

        // Spawn enemy immediately
        SpawnBasicEnemy();

        Debug.Log($"Enemy spawned with bonus health! New permanent health bonus: {permanentHealthBonus}");
    }
}
/*
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private List<GameObject> basicEnemyPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> bossPrefabs = new List<GameObject>();

    [Header("Spawn Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float basicEnemySpawnInterval = 5f;
    [SerializeField] private float bossSpawnInterval = 30f;
    [SerializeField] private float spawnHeightBasic = 10f;
    [SerializeField] private float spawnHeightBoss = 15f;
    [SerializeField] private float bossSpawnDelay = 30f;

    private float nextBasicSpawnTime;
    private float nextBossSpawnTime;
    private bool delayingBasicSpawn = false;

    bool isSpawningBosses;
    bool isSpawningBasic;
    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not set in EnemySpawner!");
            return;
        }

        if (basicEnemyPrefabs.Count == 0)
        {
            Debug.LogWarning("No basic enemy prefabs assigned to EnemySpawner!");
        }

        if (bossPrefabs.Count == 0)
        {
            Debug.LogWarning("No boss prefabs assigned to EnemySpawner!");
        }

        nextBasicSpawnTime = Time.time + basicEnemySpawnInterval;
        nextBossSpawnTime = Time.time + bossSpawnInterval;



        if (bossPrefabs.Count > 0)
            isSpawningBosses = true;

        if (basicEnemyPrefabs.Count > 0)
            isSpawningBasic = true;
    }

    void Update()
    {
        if (player == null) 
            return;


        if (isSpawningBasic)
        {
            // Check for basic enemy spawn
            if (!delayingBasicSpawn && Time.time >= nextBasicSpawnTime && basicEnemyPrefabs.Count > 0)
            {
                SpawnBasicEnemy();
                nextBasicSpawnTime = Time.time + basicEnemySpawnInterval;
            }
        }

        if(isSpawningBosses)
        {
            // Check for boss spawn
            if (Time.time >= nextBossSpawnTime && bossPrefabs.Count > 0)
            {
                SpawnBoss();
                nextBossSpawnTime = Time.time + bossSpawnInterval;
            }
        }
    }

    private void SpawnBasicEnemy()
    {
        GameObject prefab = basicEnemyPrefabs[Random.Range(0, basicEnemyPrefabs.Count)];
        Vector3 spawnPos = new Vector3(player.position.x, player.position.y + spawnHeightBasic, player.position.z);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    private void SpawnBoss()
    {
        GameObject prefab = bossPrefabs[Random.Range(0, bossPrefabs.Count)];
        Vector3 spawnPos = new Vector3(player.position.x, player.position.y + spawnHeightBoss, player.position.z);
        Instantiate(prefab, spawnPos, Quaternion.identity);

        // Delay the next basic enemy spawn
        StartCoroutine(DelayBasicSpawn());
    }

    private IEnumerator DelayBasicSpawn()
    {
        delayingBasicSpawn = true;
        yield return new WaitForSeconds(bossSpawnDelay);
        nextBasicSpawnTime = Time.time + basicEnemySpawnInterval;
        delayingBasicSpawn = false;
    }
}
*/