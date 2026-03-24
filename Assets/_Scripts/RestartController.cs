using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple controller for restarting the game from the final screen.
/// </summary>
public class RestartController : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("Name of the main gameplay scene to restart (e.g., _Scene_0)")]
    public string gameplaySceneName = "_Scene_0";

    /// <summary>
    /// Returns the player to the main level. Triggered by the RESTART button.
    /// </summary>
    public void RestartGame()
    {
        // Reset time scale to 1 (in case the game was paused)
        Time.timeScale = 1f;
        
        if (!string.IsNullOrEmpty(gameplaySceneName))
        {
            SceneManager.LoadScene(gameplaySceneName);
        }
        else
        {
            Debug.LogError("RestartController: Scene name is not specified in the Inspector!");
        }
    }
}


