using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    void Update()
    {
        if (player == null) return;

        // Check for basic enemy spawn
        if (!delayingBasicSpawn && Time.time >= nextBasicSpawnTime && basicEnemyPrefabs.Count > 0)
        {
            SpawnBasicEnemy();
            nextBasicSpawnTime = Time.time + basicEnemySpawnInterval;
        }

        // Check for boss spawn
        if (Time.time >= nextBossSpawnTime && bossPrefabs.Count > 0)
        {
            SpawnBoss();
            nextBossSpawnTime = Time.time + bossSpawnInterval;
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