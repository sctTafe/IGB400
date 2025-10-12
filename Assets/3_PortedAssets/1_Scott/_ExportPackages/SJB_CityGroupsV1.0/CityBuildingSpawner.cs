using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class CityBuildingSpawner : MonoBehaviour
{
    [Header("Building Prefabs")]
    [Tooltip("List of building prefabs to randomly spawn.")]
    public GameObject[] buildingPrefabs;

    [Header("Grid Settings")]
    [Tooltip("How many cells along X (width).")]
    public int gridWidth = 10;
    [Tooltip("How many cells along Z (depth).")]
    public int gridHeight = 10;
    [Tooltip("Size of each grid cell (world units).")]
    public float cellSize = 10f;

    [Header("Spawn Settings")]
    [Tooltip("LayerMask used to check if the grid space is occupied.")]
    public LayerMask obstacleMask;
    [Tooltip("Parent object to hold all spawned buildings.")]
    public Transform buildingParent;

    [Tooltip("The vertical range for spawn height (Y value).")]
    public Vector2 heightRange = new Vector2(-20f, 0f);

    [Tooltip("Padding inside each cell to prevent overlap with edges.")]
    public float cellPadding = 0.5f;

    [Header("Spawn Area Offset")]
    public Vector3 originOffset = Vector3.zero;

    [ContextMenu("Spawn Buildings")]
    public void SpawnBuildings()
    {
        if (buildingPrefabs == null || buildingPrefabs.Length == 0)
        {
            Debug.LogWarning("No building prefabs assigned!");
            return;
        }

        if (buildingParent == null)
            buildingParent = transform;

        int spawnCount = 0;

        // Iterate through grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Calculate center of this grid cell
                Vector3 worldPos = transform.position + originOffset + new Vector3(x * cellSize, 0, z * cellSize);

                // Volume check (cube of the same size as cell)
                Vector3 halfExtents = new Vector3((cellSize - cellPadding) * 0.5f, 100f, (cellSize - cellPadding) * 0.5f);
                Collider[] overlaps = Physics.OverlapBox(worldPos, halfExtents, Quaternion.identity, obstacleMask);

                // Skip if something occupies the cell
                if (overlaps.Length > 0)
                    continue;

                // Random prefab
                GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
                if (prefab == null)
                    continue;

                // Random rotation & height
                float randomYRot = Random.Range(0f, 360f);
                float randomYHeight = Random.Range(heightRange.x, heightRange.y);

                // Spawn building
                GameObject newBuilding = (GameObject)PrefabUtility.InstantiatePrefab(prefab, buildingParent);
                newBuilding.transform.position = new Vector3(worldPos.x, randomYHeight, worldPos.z);
                newBuilding.transform.rotation = Quaternion.Euler(0f, randomYRot, 0f);

                spawnCount++;
            }
        }

        Debug.Log($"Spawned {spawnCount} buildings successfully!");
    }

    [ContextMenu("Clear Spawned Buildings")]
    public void ClearBuildings()
    {
        if (buildingParent == null)
        {
            Debug.LogWarning("No building parent assigned!");
            return;
        }

        for (int i = buildingParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(buildingParent.GetChild(i).gameObject);
        }

        Debug.Log("Cleared all spawned buildings!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 cellPos = transform.position + originOffset + new Vector3(x * cellSize, 0, z * cellSize);
                Gizmos.DrawWireCube(cellPos, new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }
}
