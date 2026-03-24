using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform patrolRoute;
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 5.5f;

    [Header("Combat Settings")]
    public float detectionRange = 15f;
    public float attackDistance = 1.8f; 
    public float attackDelay = 1.2f;

    private NavMeshAgent _agent;
    private Transform _player;
    private Animator _animator;
    
    private List<Transform> _locations = new List<Transform>();
    private int _locationIndex = 0;
    
    private bool _isChasing = false;
    private float _lastDamageTime;

    void Start() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        
        // Optimize NavMeshAgent settings for smoother movement
        _agent.acceleration = 15f; 
        _agent.angularSpeed = 300f; 
        _agent.stoppingDistance = attackDistance - 0.2f;

        // Auto-detect player in the scene
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) _player = p.transform;
        
        InitializePatrolRoute();
    }

    void Update() 
    {
        // Stop logic if player is missing or agent is not placed on NavMesh correctly
        if (_player == null || !_agent.isOnNavMesh) return;

        float dist = Vector3.Distance(transform.position, _player.position);

        // Detect player within range
        if (dist < detectionRange) 
        {
            _isChasing = true;
        }

        // Switch between chasing and patrolling states
        if (_isChasing) 
        {
            HandleChase(dist);
        } 
        else 
        {
            HandlePatrol();
        }

        // Sync agent velocity with animator
        if (_animator != null) 
            _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    /// <summary>
    /// Pursues the player and triggers attacks when close enough.
    /// </summary>
    private void HandleChase(float dist) 
    {
        _agent.SetDestination(_player.position);
        _agent.speed = chaseSpeed;

        if (dist <= attackDistance && Time.time > _lastDamageTime + attackDelay) 
        {
            StartCoroutine(PerformAttack());
        }
    }

    /// <summary>
    /// Executes the attack animation and applies damage to the player.
    /// </summary>
    private IEnumerator PerformAttack() 
    {
        _lastDamageTime = Time.time;
        if (_animator != null) _animator.SetTrigger("Attack");

        // Wait for the animation hit point (approx 0.5s)
        yield return new WaitForSeconds(0.5f); 

        // Apply damage if the player is still within reach
        if (Vector3.Distance(transform.position, _player.position) <= attackDistance) 
        {
            GameBehavior gm = Object.FindFirstObjectByType<GameBehavior>();
            if (gm != null) gm.HP -= 1;
        }
    }

    /// <summary>
    /// Moves the agent between patrol waypoints.
    /// </summary>
    private void HandlePatrol() 
    {
        _agent.speed = patrolSpeed;
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f) 
            MoveToNextPatrolLocation();
    }

    /// <summary>
    /// Loads waypoint transforms from the patrol route parent.
    /// </summary>
    void InitializePatrolRoute() 
    {
        if (patrolRoute == null) return;
        foreach(Transform child in patrolRoute) _locations.Add(child);
        MoveToNextPatrolLocation();
    }

    /// <summary>
    /// Sets the destination to the next waypoint in the list.
    /// </summary>
    void MoveToNextPatrolLocation() 
    {
        if (_locations.Count == 0) return;
        _agent.destination = _locations[_locationIndex].position;
        _locationIndex = (_locationIndex + 1) % _locations.Count;
    }
}
