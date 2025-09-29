using UnityEngine;

public class SetYPosition : MonoBehaviour
{
    [SerializeField] float yValue = 2f;

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = yValue;
        transform.position = pos;
    }
}
