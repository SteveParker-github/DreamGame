using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IHitable
{
    private const float MAXABSORBHEALTH = 5.0f;
    private const float MAXSTUNHEALTH = 90.0f;
    [SerializeField] private List<Vector3> patrolPoints;
    [SerializeField] private float viewRadius = 10.0f;
    [SerializeField] private float viewAngle = 90.0f;
    [SerializeField] private GameObject eyeLevel;
    private Animator animator;
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private EnemyBaseState currentState;
    private EnemyStateFactory states;
    private float absorbHealth;
    private float stunHealth;
    private bool isStunned;
    private bool beenStaggered;
    private bool isStagger;
    private bool wasPatrol;
    private NavMeshAgent agent;
    private bool playerSeen;
    private PlayerController playerController;
    private Transform playerLocation;

    public List<Vector3> PatrolPoints { get => patrolPoints; }
    public EnemyBaseState CurrentState { get => currentState; set => currentState = value; }
    public bool IsStunned { get => isStunned; }
    public bool IsStagger { get => isStagger; set => isStagger = value; }
    public bool WasPatrol { get => wasPatrol; set => wasPatrol = value; }
    public NavMeshAgent Agent { get => agent; }
    public bool PlayerSeen { get => playerSeen; }
    public PlayerController PlayerController { get => playerController; }
    public Transform PlayerLocation { get => playerLocation; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        GameObject player = GameObject.Find("Player");
        playerLocation = player.transform;
        playerController = player.GetComponent<PlayerController>();
        isStunned = false;
        isStagger = false;
        beenStaggered = false;
        absorbHealth = MAXABSORBHEALTH;
        stunHealth = MAXSTUNHEALTH;
        agent = GetComponent<NavMeshAgent>();

        states = new EnemyStateFactory(this);
        currentState = states.EnemyPatrolState();
        currentState.EnterState();
    }

    private void Update()
    {
        animator.SetBool(isMovingHash, agent.velocity.magnitude > 0.1f);
        currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        if (isStunned || isStagger) return;

        DetectPlayer();
    }

    public bool Absorb(float damageMultiplier)
    {
        if (!isStunned)
        {
            return false;
        }

        absorbHealth -= Time.deltaTime * damageMultiplier;

        if (absorbHealth <= 0)
        {
            Destroy(gameObject);
        }
        return true;
    }

    public void StunDamage(float damage)
    {
        stunHealth -= damage;

        if (!beenStaggered && stunHealth <= 60)
        {
            isStagger = true;
            beenStaggered = true;
        }

        if (stunHealth <= 0)
        {
            isStunned = true;
        }
    }

    public float RemainingAbsorb()
    {
        return absorbHealth / MAXABSORBHEALTH;
    }

    private void DetectPlayer()
    {
        playerSeen = false;
        float distanceToTarget = Vector3.Distance(transform.position, playerLocation.position);

        if (distanceToTarget > viewRadius) return;

        Vector3 directionToTarget = (playerLocation.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, directionToTarget) > viewAngle / 2) return;

        RaycastHit hit;
        int layerMask = 3 << 8;
        layerMask = ~layerMask;
        if (Physics.Raycast(eyeLevel.transform.position, directionToTarget, out hit, distanceToTarget, layerMask))
        {
            //player is behind cover
            Debug.DrawRay(eyeLevel.transform.position, directionToTarget * distanceToTarget, Color.yellow);
        }
        else
        {
            //Player is seen
            Debug.DrawRay(eyeLevel.transform.position, directionToTarget * distanceToTarget, Color.red);
            playerSeen = true;
        }
    }
}
