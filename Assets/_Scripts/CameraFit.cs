using UnityEngine;

/// <summary>
/// Automatically adjusts the orthographic camera size based on a target scene width.
/// Ensures consistent game field visibility across different screen aspect ratios.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    [Header("Resolution Settings")]
    [Tooltip("Target width of the game level in Unity units.")]
    public float sceneWidth = 20f;

    private Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();
        AdjustCamera();
    }

    /// <summary>
    /// Calculates and sets the camera's orthographic size based on the current screen aspect ratio.
    /// </summary>
    public void AdjustCamera()
    {
        if (_cam == null) return;

        // Formula: Size = (Width / Aspect Ratio) / 2
        float aspectRatio = (float)Screen.width / Screen.height;
        float desiredHalfHeight = (sceneWidth / aspectRatio) * 0.5f;

        _cam.orthographicSize = desiredHalfHeight;
    }

    // Real-time updates within the editor for easier testing
#if UNITY_EDITOR
    void Update()
    {
        // Allows seeing changes immediately without entering Play Mode
        if (!Application.isPlaying) 
        {
            AdjustCamera();
        }
    }

    void OnValidate()
    {
        if (_cam == null) _cam = GetComponent<Camera>();
        AdjustCamera();
    }
#endif
}
