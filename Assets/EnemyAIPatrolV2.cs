using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIPatrolV2 : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer, obstructionLayer;

    Animator animator;
    BoxCollider boxCollider;

    //patrol
    public Vector3 destPoint;
    public Vector3 obstuctPoint;
    bool walkpointSet;
    [SerializeField] public float patrolRange;

    //enemy chasing state change
    [SerializeField] public float sightRange, attackRange;
    public bool playerInSight, playerInAttackRange;

    //FOV setup
    //public float radius; same as sightRange
    [Range(0,360)]
    public float fovAngle;
    public bool canSeePlayer;

    //speed setup
    [SerializeField] float walkSpeed, runSpeed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
        player = GameObject.Find("Robot Kyle");
        animator = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //the bottom lines of code are old method...
        //playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        if (!playerInSight && !playerInAttackRange) Patroling();
        if (playerInSight && !playerInAttackRange) Chasing();
        if (playerInSight && playerInAttackRange) Attacking();
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FOVCheck();
        }
    }

    private void FOVCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, sightRange, playerLayer);
        Transform target = rangeChecks[0].transform;  // maybe can remove this line
        Vector3 directionToTarget = (target.position - transform.position).normalized;
            //Vector3 directionToTarget = (player.transform.position - transform.position).normalized;
        if (rangeChecks.Length != 0) 
            //if (player != null)
            {
            if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                //float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
                //if player is in enemy sight range
                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, playerLayer))
                {
                    canSeePlayer = true;
                    playerInSight = true;
                    //if player is in enemy attack range
                    if (distanceToTarget < attackRange)
                    {
                        playerInAttackRange = true;
                    }
                    else
                    {
                        playerInAttackRange = false;
                    }
                }
                else
                {
                    canSeePlayer = false;
                    playerInSight = false;
                    playerInAttackRange = false;
                }
            }    
            else
            {
                canSeePlayer = false;
                playerInSight = false;
                playerInAttackRange = false;
            }          
        }
        else if (canSeePlayer) 
        {
            canSeePlayer = false;
            playerInSight = false;
            playerInAttackRange = false;
        }
    }
    
    void Chasing()
    {   
        agent.SetDestination(player.transform.position);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            animator.SetTrigger("Chase");
            agent.SetDestination(player.transform.position);
            agent.speed = runSpeed;
        }
    }

    void Attacking()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("swiping"))
        {
            animator.SetTrigger("Attack");
            agent.SetDestination(transform.position); 
        }
    }

    void Patroling()
    {
        if (!walkpointSet) FindDest();
        if (walkpointSet)
        {
            agent.SetDestination(destPoint);
            animator.SetTrigger("Patrol");
        } 
        if (Vector3.Distance(transform.position, destPoint) < attackRange) walkpointSet = false; //dafault is < 10
        agent.speed = walkSpeed;
    }

    void FindDest()
    {
        float z = Random.Range(-attackRange, attackRange);
        float x = Random.Range(-attackRange, attackRange);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z); 
        //Vector3 obstructPoint = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, sightRange, obstructionLayer))
        //if (Physics.Raycast(destPoint, transform.forward, groundLayer)) //Vector3.down
        {
            walkpointSet = true;
        }
        else
        {
            walkpointSet = false;
        }
    }

    void EnableEnemyAttack()
    {
        boxCollider.enabled = true;
    }

    void DisableEnemyAttack()
    {
        boxCollider.enabled = false;
    }
    
}
