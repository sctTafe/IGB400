using UnityEngine;
using UnityEditor;   // Required for context menu button

[ExecuteInEditMode] // Allows running in the Editor without entering Play mode
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

    [Tooltip("Height at which to raycast for collisions (e.g., above terrain).")]
    public float raycastHeight = 100f;
    [Tooltip("How far down the ray should check for collisions.")]
    public float raycastDistance = 200f;

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
                // Calculate world position
                Vector3 worldPos = transform.position + originOffset + new Vector3(x * cellSize, 0, z * cellSize);

                // Check if space is empty using Physics.CheckBox or raycast
                Vector3 checkCenter = worldPos + Vector3.up * raycastHeight;
                if (!Physics.Raycast(checkCenter, Vector3.down, out RaycastHit hit, raycastDistance, obstacleMask))
                {
                    // Nothing found in space — spawn building
                    GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
                    if (prefab == null) continue;

                    float randomYRot = Random.Range(0f, 360f);
                    GameObject newBuilding = (GameObject)PrefabUtility.InstantiatePrefab(prefab, buildingParent);
                    newBuilding.transform.position = worldPos;
                    newBuilding.transform.rotation = Quaternion.Euler(0f, randomYRot, 0f);

                    spawnCount++;
                }
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

        // Destroy children in editor safely
        for (int i = buildingParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(buildingParent.GetChild(i).gameObject);
        }

        Debug.Log("Cleared all spawned buildings!");
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the grid in editor
        Gizmos.color = Color.cyan;
        for (int x = 0; x <= gridWidth; x++)
        {
            for (int z = 0; z <= gridHeight; z++)
            {
                Vector3 cellPos = transform.position + originOffset + new Vector3(x * cellSize, 0, z * cellSize);
                Gizmos.DrawWireCube(cellPos, new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }
}
