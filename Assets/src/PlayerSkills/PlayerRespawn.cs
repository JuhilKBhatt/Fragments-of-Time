using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float fallThreshold = -6f; // Respawn Threshold
    private Vector3 respawnPosition; // Spawn Point

    [SerializeField] private GameObject spawnPoint; // Reference to the SpawnPoint GameObject
    private Rigidbody rb; // Reference to the player's Rigidbody

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component on the player

        if (spawnPoint != null)
        {
            respawnPosition = spawnPoint.transform.position; // Set the respawn position to the SpawnPoint's position
        }
        else
        {
            Debug.LogWarning("SpawnPoint not assigned in the Inspector.");
            respawnPosition = transform.position; // If no SpawnPoint is assigned, fall back to the player's initial position
        }
    }

    private void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        // Reset the player's velocity to zero to prevent momentum from pushing them into the ground
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Stop any existing momentum
        }

        // Move the player to the respawn position
        transform.position = respawnPosition;
    }
    public void justMovePosition(Vector3 newPosition)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }

        transform.position = newPosition;
    }

    public void SetRespawn(Vector3 point)
    {
        respawnPosition = point;
        Respawn();
    }
    public void OnlySetRespawn(Vector3 point)
    {
        respawnPosition = point;
    }
}