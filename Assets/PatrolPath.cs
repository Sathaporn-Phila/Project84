using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolPath : MonoBehaviour 
{
    public GameObject player;
    private NavMeshAgent agent;
    private bool blocked = false;

    public Transform[] stopPoints;
    public int destPoint = 0;
    bool walkpointSet = true;

    [SerializeField] 
    public LayerMask groundLayer, playerLayer;

    Animator animator;
    BoxCollider boxCollider;
    
    [SerializeField] 
    public float sightRange, attackRange;
    public bool playerInSight, playerInAttackRange;

    [Range(0,360)]
    public float fovAngle;

    void Start () {
        agent = GetComponentInChildren<NavMeshAgent>();
        player = GameObject.Find("Robot Kyle");
        animator = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();
        StartCoroutine(FOVRoutine());
        agent.autoBraking = false;
        walkpointSet = true;
        agent.SetDestination(stopPoints[0].position);
    }

    void Update () 
    {
        FOVCheck();
        if (!playerInSight && !playerInAttackRange) Patroling();
        if (playerInSight && !playerInAttackRange) Chasing();
        if (playerInSight && playerInAttackRange) Attacking();
        Debug.DrawLine(transform.position, agent.destination, blocked ? Color.red : Color.green);
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
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, groundLayer))
                {
                    playerInSight = true;

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
        else if (playerInSight) 
        {
            playerInSight = false;
            playerInAttackRange = false;
        }
    }

    void GotoNextPoint() 
    {
        if (stopPoints.Length == 0)
            return;

        agent.destination = stopPoints[destPoint].position;
        agent.SetDestination(agent.destination);
    }

    void Patroling()
    {   
        if (!agent.pathPending && agent.remainingDistance < 5) 
        {
            animator.SetTrigger("Patrol");
            destPoint = (destPoint + 1) % stopPoints.Length;
            GotoNextPoint();
        }
    }

    /*void Detecting()
    {   
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            animator.SetTrigger("Chase");
        }
    }*/

    void Chasing()
    {   
        agent.SetDestination(transform.position); 
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            animator.SetTrigger("Chase");
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            agent.SetDestination(player.transform.position);
        }
        
    }

    void Attacking()
    {
        agent.SetDestination(transform.position);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("swiping"))
        {
            animator.SetTrigger("Attack");
        }
    }
}

    