using UnityEngine;
using StarterAssets;

/// <summary>
/// Extension of the standard player controller: handles shooting mechanics, 
/// ammo management, visual effects, and IK hand correction.
/// </summary>
public class PlayerBehaviour : MonoBehaviour
{
    private StarterAssetsInputs _input; 
    private GameBehavior _gameManager;
    private Animator _animator; 
    private CharacterController _controller; 

    [Header("Shooting Settings")]
    [Tooltip("Bullet prefab with a Rigidbody component")]
    public GameObject bullet;      
    [Tooltip("Spawn point for the bullet (gun muzzle)")]
    public Transform firePoint;    
    public float bulletSpeed = 50f; 
    
    [Header("Player State")]
    public GameObject visualGun;   // Gun model in the character's hand
    public int ammoCount = 0;      // Current ammo amount
    public bool hasGun = false;    // Flag to check if the player has a weapon
    public GameObject muzzleFlash; // Muzzle flash visual effect

    // Delegate and event to extend jump logic without modifying the base asset
    public delegate void JumpingEvent();
    public event JumpingEvent playerJump;

    void Start()
    {
        // Getting references to Starter Assets components
        _input = GetComponent<StarterAssetsInputs>();
        if (_input == null) _input = GetComponentInParent<StarterAssetsInputs>();
        
        _animator = GetComponent<Animator>(); 
        _controller = GetComponent<CharacterController>();

        // Dynamic search for the Game Manager
        _gameManager = Object.FindFirstObjectByType<GameBehavior>();
    }

    void Update()
    {
        if (_input == null) return;
        
        // Trigger jump event (Observer Pattern)
        if (_input.jump) playerJump?.Invoke();

        // Check conditions for shooting: Left Click, gun possession, and ammo availability
        if (Input.GetMouseButtonDown(0) && hasGun && ammoCount > 0)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Creates a bullet, configures its physics, and ignores collisions with the player.
    /// </summary>
    private void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
            Invoke("HideFlash", 0.05f); // Short flash duration
        }

        // Spawn the bullet
        GameObject newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        
        // Ignore collision between the bullet and the player's CharacterController to avoid self-hits
        Collider bulletCollider = newBullet.GetComponent<Collider>();
        if (bulletCollider != null && _controller != null)
        {
            Physics.IgnoreCollision(bulletCollider, _controller);
        }

        Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
        if (bulletRB != null)
        {
            bulletRB.linearDamping = 0; 
            bulletRB.useGravity = false;
            // Using linearVelocity (standard for Unity 6)
            bulletRB.linearVelocity = firePoint.forward * bulletSpeed;
        }

        ammoCount--;
        
        // If out of ammo, check for Game Over condition after 2 seconds
        if (ammoCount <= 0) Invoke("CheckGameOver", 2f);
        
        // Optimization: remove bullet after some time if it hasn't hit anything
        Destroy(newBullet, 3f);
        Debug.DrawRay(firePoint.position, firePoint.forward * 10f, Color.red, 2f);
    }

    private void CheckGameOver()
    {
        // If ammo is depleted and enemies are still alive, trigger game over
        if (GameObject.FindGameObjectWithTag("Enemy") != null && _gameManager != null)
        {
            _gameManager.GameOver(false);
        }
    }

    /// <summary>
    /// Direct bone manipulation via LateUpdate.
    /// Positions the arm with the gun correctly after the walking animation plays.
    /// </summary>
    /*
    void LateUpdate()
    {
        if (hasGun && _animator != null)
        {
            // Finding the arm bone in the skeleton
            Transform shoulder = _animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            if (shoulder != null) 
                shoulder.localRotation = Quaternion.Euler(-10, 90, 90); 
        }
    }
    */

    void HideFlash() { if (muzzleFlash != null) muzzleFlash.SetActive(false); }
    
    // Stubs for Starter Assets animation events
    public void OnFootstep(AnimationEvent animationEvent) { }
    public void OnLand(AnimationEvent animationEvent) { }
}
