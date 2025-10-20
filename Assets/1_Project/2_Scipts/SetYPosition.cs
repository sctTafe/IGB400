using UnityEngine;

public class SetYPosition : MonoBehaviour
{
    [SerializeField] float yValue = 2f;
    [SerializeField] float moveSpeed = 3f;

    void Update()
    {
        Vector3 pos = transform.position;

        if (Mathf.Abs(pos.y - yValue) > 0.001f)
        {
            pos.y = Mathf.MoveTowards(pos.y, yValue, moveSpeed * Time.deltaTime);
            transform.position = pos;
        }
    }
}
