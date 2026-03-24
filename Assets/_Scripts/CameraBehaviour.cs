using UnityEngine;

/// <summary>
/// Third-Person Camera (TPS) implementation that follows the player 
/// with a specific offset and always stays focused on them.
/// </summary>
public class CameraBehaviour : MonoBehaviour
{
    [Header("Position Settings")]
    [Tooltip("Distance and height of the camera relative to the player's center")]
    public Vector3 camOffset = new Vector3(0f, 1.2f, -2.6f); 

    private Transform _target; 

    void Start()
    {
        // Dynamic player search in the scene. 
        // In larger projects, assigning the reference via Inspector is better for optimization.
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            _target = playerObj.transform;
        }
    }

    // Using LateUpdate is critical for smooth camera movement.
    // This method is called after all objects have moved in Update,
    // preventing camera jittering while following the player.
    void LateUpdate() 
    {
        if (_target == null) return;

        // Calculate camera position in player's local space and convert to world space.
        // This keeps the camera angle consistent even when the player rotates.
        this.transform.position = _target.TransformPoint(camOffset);

        // Aim the camera's gaze at the player's transform center.
        this.transform.LookAt(_target);
    }
}
