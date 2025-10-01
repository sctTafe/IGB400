using UnityEngine;

public class ScrapPickupSpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject prefabToSpawn;   // Prefab to spawn
    [SerializeField] private Transform minYPoint;        // Lower Y bound
    [SerializeField] private Transform maxYPoint;        // Upper Y bound

    [Header("Spawn Settings")]
    [SerializeField] private int spawnCount = 10;        // How many to spawn
    [SerializeField] private float spawnRadius = 5f;     // Radius on XZ plane

    [ContextMenu("Spawn Prefabs")]

    private void OnEnable()
    {
        SpawnPrefabs();
    }

    public void SpawnPrefabs()
    {
        if (prefabToSpawn == null || minYPoint == null || maxYPoint == null)
        {
            Debug.LogWarning("Spawner missing setup! Assign prefab & Y transforms.");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            // Pick random Y between min & max
            float yPos = Random.Range(minYPoint.position.y, maxYPoint.position.y);

            // Pick random position in circle (XZ plane)
            Vector2 circle = Random.insideUnitCircle * spawnRadius;

            Vector3 spawnPos = new Vector3(
                transform.position.x + circle.x,  // Offset from spawner center
                yPos,
                transform.position.z + circle.y
            );

            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        }
    }
}