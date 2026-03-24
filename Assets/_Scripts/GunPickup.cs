using UnityEngine;

/// <summary>
/// Controls the "Gun" pickup item in the scene: its rotation 
/// and player collection logic.
/// </summary>
public class GunPickup : MonoBehaviour
{
    [Header("Visual Settings")]
    [Tooltip("Rotation speed of the item in the air to attract player attention")]
    public float rotationSpeed = 60f;

    [Header("Ammo Settings")]
    [Tooltip("Amount of ammo the player receives when picking up this item")]
    public int ammoToGive = 3;

    void Update()
    {
        // Smoothly rotate the item around the vertical axis. 
        // Time.deltaTime makes rotation frame-rate independent.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Processes the event when the player enters the item's trigger zone.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        // Check the tag of the object entering the trigger
        if (other.CompareTag("Player"))
        {
            // Access the player's PlayerBehaviour component
            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            
            if (player != null)
            {
                // Enable the player's shooting mechanics
                player.hasGun = true;
                
                // Enable the visual weapon model in the character's hands
                if (player.visualGun != null)
                    player.visualGun.SetActive(true);
                
                // Replenish ammunition
                player.ammoCount += ammoToGive; 

                // Remove the pickup item from the scene after collection
                Destroy(this.gameObject);
            }
        }
    }
}

