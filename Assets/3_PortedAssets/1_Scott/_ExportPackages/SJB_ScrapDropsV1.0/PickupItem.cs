using UnityEngine;
/// <summary>
/// Script for the Collectible pickup item.
/// Controls how the player can collect it and the veward value it generates 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PickupItem : MonoBehaviour
{
    [Header("Scrap Values")]
    [SerializeField] int _ScarpValueMin  = 8;
    [SerializeField] int _ScarpValueMax = 12;
    [Header("Pickup Settings")]
    [SerializeField] private float attractRadius = 150f;   // Radius within which the item moves to player
    [Tooltip("The Max Spped the pickup will accelerate up to")]
    [SerializeField] private float moveSpeed = 300f;       // Base move speed
    [SerializeField] private float acceleration = 20f;   // How quickly it accelerates

    private Rigidbody rb;
    private Transform targetPlayer;
    private float currentSpeed = 0f;

    private bool isAttracted = false;

    Scrap_TriggerRelay Scrap_TriggerRelay;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;  // Make sure it drops naturally
        Scrap_TriggerRelay = GetComponent<Scrap_TriggerRelay>();
    }

    private void Update()
    {
        if (isAttracted && targetPlayer != null)
        {
            // Accelerate speed
            //currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, acceleration * Time.deltaTime);
            // Exponential ease-in
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, 1f - Mathf.Exp(-acceleration * Time.deltaTime));

            // Move toward the player
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * currentSpeed * Time.deltaTime);
        }
        else
        {
            // Look for players within radius
            Collider[] hits = Physics.OverlapSphere(transform.position, attractRadius);
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    targetPlayer = hit.transform;
                    isAttracted = true;

                    // Turn off gravity so it doesn�t keep falling while flying to the player
                    rb.useGravity = false;
                    rb.linearVelocity = Vector3.zero; // stop downward momentum

                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected(other.gameObject);
            Destroy(gameObject); // Destroy the pickup after collection
        }
    }

    // Dummy function for now
    private void OnCollected(GameObject player)
    {
        Debug.Log($"{gameObject.name} collected by {player.name}!");
        Scrap_TriggerRelay.fn_AddScore(Random.Range(_ScarpValueMin, _ScarpValueMax));
    }

    // Draw radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractRadius);
    }
}
