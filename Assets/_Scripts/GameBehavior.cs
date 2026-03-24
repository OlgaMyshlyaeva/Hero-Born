using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Main Game Manager. Handles HUD, player health, and scene transitions.
/// </summary>
public class GameBehavior : MonoBehaviour 
{
    [Header("Player Interface (HUD)")]
    public TMP_Text healthText;
    public TMP_Text ammoText;
    
    [Header("Color Settings")]
    public Color normalHealthColor = Color.white;
    public Color damageHealthColor = Color.red;

    [Header("Final Scene Names")]
    [Tooltip("Name of the victory scene")]
    public string winSceneName = "_Scene_Win";
    [Tooltip("Name of the game over scene")]
    public string lossSceneName = "_Scene_Loss";

    [SerializeField] private int _playerHP = 3;
    private PlayerBehaviour _playerScript;

    public int HP
    {
        get { return _playerHP; }
        set
        {
            _playerHP = value;
            UpdateHealthUI();
            
            // Flash health text when taking damage
            if (healthText != null) StartCoroutine(FlashHealthText());

            if(_playerHP <= 0)
            {
                GameOver(false); 
            }
        }
    }

    void Start()
    {
        Time.timeScale = 1.0f; // Reset time scale (pause)

        // Find the player object in the scene
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) _playerScript = playerObj.GetComponent<PlayerBehaviour>();

        UpdateHealthUI();
    }

    void Update()
    {
        // Update ammo display
        if (_playerScript != null && ammoText != null)
        {
            ammoText.text = "Ammo: " + _playerScript.ammoCount;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null) healthText.text = "Health: " + _playerHP;
    }

    private IEnumerator FlashHealthText()
    {
        healthText.color = damageHealthColor;
        yield return new WaitForSeconds(0.2f);
        healthText.color = normalHealthColor;
    }

    /// <summary>
    /// Method to load end-game scenes
    /// </summary>
    public void GameOver(bool won)
    {
        // Unlock the cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (won)
        {
            SceneManager.LoadScene(winSceneName);
        }
        else
        {
            SceneManager.LoadScene(lossSceneName);
        }
    }
}
