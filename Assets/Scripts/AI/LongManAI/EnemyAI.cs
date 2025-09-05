using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    private CharStatusManager playerStatus; // Player Status

    private LMBehaviourTree behaviorTree;
    public float detectionRange = 20f;
    public float hearingThreshold = 35;
    private bool isSoundDetected = false;

    private float stunDuration = 5f;
    private float stunTimer = 0f;
    private bool wasHit = false;

    public bool isStunned = false; // Currently is Stunned
    public bool isChasing = false; // Currently is Chasing
    public bool isAttacking = false; // Currently is Attacking
    public bool isWalking;

    public Transform player;
    private NavMeshAgent navMeshAgent;

    [Header("Line of Sight Settings")]
    public float fieldOfViewAngle = 120f; 
    public LayerMask detectionLayerMask;
    public LayerMask obstructionMask;
    public Vector3 sightDirectionOffset = Vector3.zero;

    [Header("Roaming Settings")]
    public List<Vector3> roamingPoints; // List of predefined points
    private Queue<Vector3> unvisitedPoints; // Queue of unvisited points
    private Vector3 currentTarget;
    private bool hasTarget = false;
    private float reachThreshold = 1.5f;

    [Header("Attack Settings")]
    private bool canAttack = true;
    public float attackRange = 3.0f;
    private float attackAnimStartDetermination = 2.8f; // Set Up In Code, Animation Start Attack action, IDK how to say, KEEP THIS VALUE
    public float damageCooldown = 4.31f; // How long to cause next damage to player, KEEP THIS VALUE
    public float attackDamage = 24f; // Attack Damage

    private float attackAnimationLength = 4.2f;

    private Animator animator;



    private void Start()
    {
        // Get Component
        animator = GetComponent<Animator>();
        playerStatus = player.GetComponent<CharStatusManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Get Player
        player = GameObject.FindWithTag("Player").transform;

        // Nav Mesh Update Rotation
        navMeshAgent.updateRotation = false;

        // Behavior Tree
        behaviorTree = new LMBehaviourTree(this);

        // Shuffle Points
        ShuffleAndInitializePoints();

        // Debug Logs
        Debug.Log(player != null ? "Player found!" : "Player not found!");
        Debug.Log(navMeshAgent != null ? "NavMeshAgent found!" : "NavMeshAgent not found!");
        Debug.Log(behaviorTree != null ? "BehaviorTree initialized!" : "BehaviorTree failed to initialize!");
    }


    private void Update()
    {
        // Skip other behaviors while stunned
        if (isStunned)
        {
            AnimatorStopAttacking();
            StopDamage();

            // Stun Effect
            HandleStunEffect();
            return;
        }

        // Attack Will Not Move, Or Can Move
        if (isAttackStopMove())
        {
            return;
        }

        // If Player is within chase range but out of attack range, chase the player
        if (IsPlayerInLineOfSight() && !IsWithinAttackRange(player) && !isAttacking)
        {
            StartChase();
        }
        // If Player is in attack range, attack the player
        else if (IsWithinAttackRange(player) && !isAttacking)
        {
            StartAttack();
        }
        // If Player is out of chase range, stop chasing
        else if (!IsPlayerInLineOfSight())
        {
            StopChase();
        }

        // Update Animator with current states
        animator.SetBool("isChasing", isChasing);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isStunned", isStunned);

        if (navMeshAgent.velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Quaternion.Euler(-1 * sightDirectionOffset) * navMeshAgent.velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * navMeshAgent.angularSpeed);
        }

        behaviorTree.Evaluate();
    }




    // Attack Stop Move
    private bool isAttackStopMove()
    {
        // Attacking Stop Move, Or Can Move
        if (isAttacking)
        {
            navMeshAgent.isStopped = true;
            return true;
        }
        else
        {
            navMeshAgent.isStopped = false;
            return false;
        }
    }




    // Start Attack
    private void StartAttack()
    {
        isChasing = false;
        isAttacking = true;
        isStunned = false;

        if (!IsInvoking("DealDamage"))
        {
            StartDamage(); // Damage
        }

        AttackObject(player); // Attack
    }




    // Start Chase
    private void StartChase()
    {
        isChasing = true;
        isAttacking = false;
        isStunned = false;

        // Start Chasing
        AnimatorStartChasing(); // Animator

        StopDamage(); // Stop Damage

        ChaseTarget(); // Chase
    }

    // Stop Chase
    private void StopChase()
    {
        isChasing = false;
        isAttacking = false;
        isStunned = false;

        // Stop Chasing
        AnimatorStopChasing(); // Animator

        StopDamage(); // Stop Damage

        Roam();
    }




    // Animator Behaviour

    // Animator Stunning
    private void AnimatorStartStunning() // Start
    {
        // Set Animator
        animator.SetBool("isChasing", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isStunned", true);
        animator.SetBool("isStunIdle", true);
    }




    // Animator Chasing
    private void AnimatorStartChasing() // Start 
    {
        // Set Animator Bools
        animator.SetBool("isChasing", true);
        animator.SetBool("isWalking", false);
    }

    private void AnimatorStopChasing() // Stop
    {
        // Set Animator Bool
        animator.SetBool("isChasing", false);
    }


    // Animator Attacking
    private void AnimatorStartAttacking() // Start
    {
        animator.SetBool("isAttacking", true);
        animator.SetBool("isChasing", false);
        animator.SetBool("isWalking", false);
    }

    private void AnimatorStopAttacking() // Stop
    {
        animator.SetBool("isAttacking", false);
    }




    // Attack Range
    public bool IsWithinAttackRange(Transform target)
    {
        return Vector3.Distance(transform.position, target.position) <= attackRange;
    }


    // Sight
    public bool IsPlayerInLineOfSight()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, detectionRange, detectionLayerMask);

        if (rangeChecks.Length > 0)
        {
            foreach (Collider col in rangeChecks)
            {
                if (col.CompareTag("Player"))
                {
                    Vector3 directionToPlayer = (col.transform.position - transform.position).normalized;

                    Vector3 adjustedForward = Quaternion.Euler(sightDirectionOffset) * transform.forward;
                    float angleToPlayer = Vector3.Angle(adjustedForward, directionToPlayer);

                    if (angleToPlayer < fieldOfViewAngle / 2f)
                    {
                        float distanceToPlayer = Vector3.Distance(transform.position, col.transform.position);
                        Debug.DrawRay(transform.position, directionToPlayer * distanceToPlayer, Color.red);

                        if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionMask))
                        {
                            Debug.Log("Player detected in line of sight!");
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }




    // Sound Detection
    public bool IsSoundDetected()
    {
        return isSoundDetected;
    }

    public void ResetSoundDetection()
    {
        isSoundDetected = false;
    }




    // Chase Target
    public void ChaseTarget()
    {
        isWalking = false;
        if (player != null && navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.position);

            Debug.Log("Chasing the player using NavMesh pathfinding.");
        }
    }




    // Roam
    public void Roam()
    {
        isWalking = true;
        animator.SetBool("isWalking", true);
        if (!hasTarget || Vector3.Distance(transform.position, currentTarget) <= reachThreshold)
        {
            SetNextRoamingTarget();
        }

        if (hasTarget && navMeshAgent.isActiveAndEnabled && !navMeshAgent.hasPath)
        {
            navMeshAgent.SetDestination(currentTarget);
            Debug.Log("Moving to roaming target: " + currentTarget);
        }
    }

    // Next Roaming Target
    private void SetNextRoamingTarget()
    {
        while (unvisitedPoints.Count > 0)
        {
            currentTarget = unvisitedPoints.Dequeue();
            NavMeshPath path = new NavMeshPath();

            if (navMeshAgent.CalculatePath(currentTarget, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                hasTarget = true;
                navMeshAgent.SetPath(path);
                return;
            }
        }

        ShuffleAndInitializePoints();
    }

    // Shuffle Initialize Points
    private void ShuffleAndInitializePoints()
    {
        List<Vector3> shuffledPoints = new List<Vector3>(roamingPoints);
        System.Random rng = new System.Random();
        int n = shuffledPoints.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Vector3 value = shuffledPoints[k];
            shuffledPoints[k] = shuffledPoints[n];
            shuffledPoints[n] = value;
        }
        unvisitedPoints = new Queue<Vector3>(shuffledPoints);
        hasTarget = false;
    }




    // Take Damage
    public void TakeDamage(float damage)
    {
        wasHit = true;
    }




    // Stun
    public bool Stun()
    {
        if (wasHit)
        {
            Debug.Log("Stun");
            isStunned = true;
            stunTimer = stunDuration;
            navMeshAgent.isStopped = true;

            Debug.Log("Enemy stunned for " + stunDuration + " seconds.");
            wasHit = false;
            return true;
        }
        else
        {
            return false;
        }
    }


    // Stun Effect
    private void HandleStunEffect()
    {
        AnimatorStartStunning();

        stunTimer -= Time.deltaTime;

        // Stop Stunned
        if (stunTimer <= 0)
        {
            isStunned = false;
            navMeshAgent.isStopped = false;

            animator.SetBool("isStunned", false);
            animator.SetBool("isStunIdle", false);

            wasHit = false;
        }
    }




    // Start Damage player
    public void StartDamage()
    {
        if (playerStatus != null && !IsInvoking("DealDamage"))
        {
            InvokeRepeating("DealDamage", attackAnimStartDetermination, damageCooldown); // Damage Interval
        }
    }

    // Deal Damage
    void DealDamage()
    {
        if (playerStatus != null)
        {
            // Check Player Is In Attack Range First
            if (IsWithinAttackRange(player))
            {
                // Taken Damage
                Debug.Log("Attacking the player, Taken Damage.");
                playerStatus.TakeDamage(attackDamage);
            }
        }
    }

    // Stop Damage player
    void StopDamage()
    {
        // Debug.Log("Stop Damage");
        CancelInvoke("DealDamage");
    }




    // Check And Attack
    public void CheckAndAttackTarget(GameObject target)
    {
        if (target.CompareTag("Player") && canAttack)
        {
            AttackObject(player);
        }
    }

    // Attack
    public void AttackObject(Transform target)
    {
        isWalking = false;
        if (target.CompareTag("Player")) // Player
        {
            // Set Attack True
            isAttacking = true;

            // Set Animator Bools
            AnimatorStartAttacking();


            // Handle Reset Attack
            if (IsWithinAttackRange(player))
            {
                StartCoroutine(ResetAttackAfterAnimation());
            }
        }
    }

    // Reset Attack After Animation
    private IEnumerator ResetAttackAfterAnimation()
    {
        // Wait for the attack animation to complete
        yield return new WaitForSeconds(attackAnimationLength);

        // Reset Attack
        ResetAttack();
    }

    // Reset Attack
    private void ResetAttack()
    {
        // Set Attack False
        isAttacking = false;

        // Resume Chasing
        if (IsPlayerInLineOfSight() && !IsWithinAttackRange(player))
        {
            ChaseTarget();
        }
    }



    // On Trigger Enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SoundSphere"))
        {
            isSoundDetected = true;  
            
        }
    }




    // Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);


        Vector3 adjustedForward = Quaternion.Euler(sightDirectionOffset) * transform.forward;

        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * adjustedForward;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * adjustedForward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionRange);

        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, player.position);
        }

        if (IsPlayerInLineOfSight())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}