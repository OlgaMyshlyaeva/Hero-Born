using UnityEngine;
using UnityEngine.SceneManagement; // Essential for scene management

/// <summary>
/// Manages the main menu logic for the "Hero Born" prototype.
/// Includes methods for scene transitions and application exit.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("The name of the main gameplay scene to load.")]
    public string gameSceneName = "_Scene_0";

    /// <summary>
    /// Starts the game by loading the specified gameplay scene.
    /// Make sure the scene is added to 'Scenes In Build' in Build Settings.
    /// </summary>
    public void StartGame()
    {
        // Resume time in case it was paused in a previous session
        Time.timeScale = 1f;

        // Check if the scene name is valid before loading
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("MainMenuController: Game Scene Name is not assigned!");
        }
    }

    /// <summary>
    /// Closes the game application. 
    /// Note: This only works in standalone builds, not in the Unity Editor.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Exiting the Hero Born application...");
        
        // Closes the actual application (.exe / .apk)
        Application.Quit();

        // Support for exiting Play Mode while testing in Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
