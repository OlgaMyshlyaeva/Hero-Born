using UnityEngine;

/// <summary>
/// Handles bullet logic: impact detection, enemy destruction, and triggering the win state.
/// </summary>
public class BulletBehavior : MonoBehaviour
{
    [Header("Settings")]
    public float onscreenDelay = 3f; // Lifetime before auto-destruction

    void Start()
    {
        // Auto-cleanup to prevent memory leaks if the bullet misses
        Destroy(this.gameObject, onscreenDelay);
    }

    /// <summary>
    /// Physical collision (e.g., hitting a solid Wall or Floor).
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        HandleImpact(collision.gameObject);
    }

    /// <summary>
    /// Trigger collision (used for projectiles or passing through detection zones).
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Ignore the enemy's vision spheres or other detection triggers
        if (other.isTrigger) return;
        
        HandleImpact(other.gameObject);
    }

    /// <summary>
    /// Core logic for processing hits and calling the Game Manager.
    /// </summary>
    private void HandleImpact(GameObject hitObject)
    {
        // Avoid self-collision with other bullets
        if (hitObject.CompareTag("Bullet")) return;

        // Check if the hit object or its parent is tagged as "Enemy"
        if (hitObject.CompareTag("Enemy") || hitObject.transform.root.CompareTag("Enemy"))
        {
            // Reference the main Game Manager to trigger the GameOver state
            GameBehavior gameManager = Object.FindFirstObjectByType<GameBehavior>();

            if (gameManager != null)
            {
                // Call the victory method from your GameBehavior script
                gameManager.GameOver(true);
            }

            // Destroy the entire enemy prefab and the bullet itself
            Destroy(hitObject.transform.root.gameObject);
            Destroy(this.gameObject);
        }
        else 
        {
            // Destroy the bullet if it hits obstacles (Walls, Floors, etc.)
            Destroy(this.gameObject);
        }
    }
}


