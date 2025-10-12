using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshColliderTools : MonoBehaviour
{
#if UNITY_EDITOR
    [ContextMenu("Add Mesh Colliders to Children")]
    private void AddMeshCollidersToChildren()
    {
        int addedCount = 0;
        foreach (var renderer in GetComponentsInChildren<MeshRenderer>(true))
        {
            GameObject go = renderer.gameObject;
            if (go.GetComponent<MeshCollider>() == null)
            {
                MeshFilter mf = go.GetComponent<MeshFilter>();
                if (mf != null && mf.sharedMesh != null)
                {
                    Undo.AddComponent<MeshCollider>(go);
                    addedCount++;
                }
            }
        }

        Debug.Log($"✅ Added MeshColliders to {addedCount} child objects under '{name}'.");
    }

    [ContextMenu("Remove Mesh Colliders from Children")]
    private void RemoveMeshCollidersFromChildren()
    {
        int removedCount = 0;
        foreach (var collider in GetComponentsInChildren<MeshCollider>(true))
        {
            Undo.DestroyObjectImmediate(collider);
            removedCount++;
        }

        Debug.Log($"🗑️ Removed {removedCount} MeshColliders from '{name}' and its children.");
    }
#endif
}