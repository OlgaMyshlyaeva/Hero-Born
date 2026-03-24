using UnityEngine;

/// <summary>
/// Logic for health pickups. Increases player health upon collision.
/// </summary>
public class ItemBehavior : MonoBehaviour
{
    private GameBehavior _gameManager;

    void Start()
    {
        // Finding the Game Manager in the current scene
        _gameManager = Object.FindFirstObjectByType<GameBehavior>();
    }

    /// <summary>
    /// Executes when an object enters the trigger zone.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        // Check if the object is the Player by its Tag
        if (other.CompareTag("Player"))
        {
            if (_gameManager != null)
            {
                // Increment player HP through the Game Manager property
                _gameManager.HP += 1;
            }

            // Remove the item from the scene after use
            Destroy(this.gameObject);
        }
    }
}

