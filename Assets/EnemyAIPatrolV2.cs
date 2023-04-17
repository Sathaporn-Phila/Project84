using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIPatrolV2 : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;

    [SerializeField] public LayerMask groundLayer, playerLayer, obstructionLayer;

    Animator animator;
    BoxCollider boxCollider;

    //patrol
    public Vector3 destPoint;
    public Vector3 obstructPoint;
    bool walkpointSet;
    [SerializeField] public float patrolRange;

    //enemy chasing state change
    [SerializeField] public float sightRange, attackRange;
    public bool playerInSight, playerInAttackRange;

    //FOV setup
    //public float radius; same as sightRange
    [Range(0,360)]
    public float fovAngle;
    //public bool canSeePlayer;

    //speed setup
    [SerializeField] float walkSpeed, runSpeed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
        player = GameObject.Find("Robot Kyle");
        animator = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();
        StartCoroutine(FOVRoutine());
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
        
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;  // maybe can remove this line
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                //float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
                
                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                {
                    //if player is in enemy sight range
                    
                    //canSeePlayer = true;
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
                    //canSeePlayer = false;
                    playerInSight = false;
                    playerInAttackRange = false;
                }
            }    
            else
            {
                //canSeePlayer = false;
                playerInSight = false;
                playerInAttackRange = false;
            }          
        }
        else if (playerInSight) 
        {
            //canSeePlayer = false;
            playerInSight = false;
            playerInAttackRange = false;
        }
    }
    
    void Chasing()
    {   
        //agent.SetDestination(player.transform.position);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            animator.SetTrigger("Chase");
            agent.SetDestination(player.transform.position);
        }
        agent.speed = runSpeed;
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
        if (Vector3.Distance(transform.position, destPoint) < 10) walkpointSet = false; //dafault is < 10
        agent.speed = walkSpeed;
    }

    void FindDest()
    {
        float z = Random.Range(-patrolRange, patrolRange);
        float x = Random.Range(-patrolRange, patrolRange);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z); 
        Vector3 obstructPoint = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer) && /*!*/Physics.Raycast(transform.position, obstructPoint, out hit, sightRange)) 
        //if (Physics.Raycast(destPoint, transform.forward, groundLayer)) //Vector3.down
        {
            walkpointSet = true;
        }
        else
        {
            walkpointSet = false;
        }
    }

    public void EnableEnemyAttack()
    {
        boxCollider.enabled = true;
    }

    public void DisableEnemyAttack()
    {
        boxCollider.enabled = false;
    }
    
}
