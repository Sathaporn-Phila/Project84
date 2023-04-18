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

    //check if blocked
    public Transform endRay;
    private NavMeshHit hit;
    private bool blocked = false;

    //patrol
    public Vector3 destPoint;
    //public Vector3 obstructPoint;
    public bool walkpointSet;
    [SerializeField] public float minPatrolRange, maxPatrolRange, blockRange;
    
    //enemy chasing state change
    [SerializeField] public float sightRange, attackRange;
    public bool playerInSight, playerInAttackRange;

    //FOV setup
    [Range(0,360)]
    public float fovAngle;

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

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                {
                    playerInSight = true;

                    if (distanceToTarget <= attackRange)
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
                    playerInSight = false;
                    playerInAttackRange = false;
                }
            }    
            else
            {
                playerInSight = false;
                playerInAttackRange = false;
            }          
        }
        /*else if (playerInSight) 
        {
            playerInSight = false;
            playerInAttackRange = false;
        }*/
    }
    
    void Chasing()
    {   
        //agent.SetDestination(player.transform.position);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            animator.SetTrigger("Chase");
        }
        agent.SetDestination(player.transform.position);
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
        if (Vector3.Distance(transform.position, destPoint) <= minPatrolRange) walkpointSet = false; //dafault is < 10
        agent.speed = walkSpeed;
    }

    void FindDest()
    {
        float z = Random.Range(-maxPatrolRange, maxPatrolRange);
        float x = Random.Range(-maxPatrolRange, maxPatrolRange);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z); 
        //Vector3 obstructPoint = transform.TransformDirection(Vector3.forward);
        //RaycastHit hit;

        blocked = NavMesh.Raycast(transform.position, endRay.position, out hit, groundLayer); //NavMesh.AllAreas

        //if (Physics.Raycast(destPoint, transform.forward, groundLayer)) //Vector3.down
        //if (Physics.Raycast(destPoint, transform.forward, groundLayer) && Physics.Raycast(transform.position, obstructPoint, out hit, sightRange)) 
        if (/*Physics.Raycast(destPoint, Vector3.down, groundLayer) && */!blocked) 
        {
            walkpointSet = true;
            /*Debug.DrawLine(transform.position, transform.forward * minPatrolRange, blocked ? Color.red : Color.green);

            if (blocked)
            {
                Debug.DrawRay(hit.position, Vector3.up, Color.red);
            }*/
        }   
        else
        {
            walkpointSet = false;
        }
    }

    /*public void EnableEnemyAttack()
    {
        boxCollider.enabled = true;
    }

    public void DisableEnemyAttack()
    {
        boxCollider.enabled = false;
    }*/

    /*
    public Transform endRay;
    private NavMeshHit hit;
    private bool blocked = false;
        
    void Update()
    {
        blocked = NavMesh.Raycast(transform.position, endRay.position, out hit, NavMesh.AllAreas);
        Debug.DrawLine(transform.position, endRay.position, blocked ? Color.red : Color.green);

        if (blocked)
            Debug.DrawRay(hit.position, Vector3.up, Color.red);
    }

    */

    
}
