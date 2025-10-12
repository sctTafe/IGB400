using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CombineChildMeshes : MonoBehaviour
{

    [ContextMenu("Combind Mesh Colliders of Children")]
    private void CombindColliderMesh()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);

        MeshFilter parentFilter = GetComponent<MeshFilter>();
        parentFilter.mesh = combinedMesh;

        gameObject.AddComponent<MeshCollider>().sharedMesh = combinedMesh;
        gameObject.SetActive(true);
    }
}
