using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAIPatrol : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer;

    Animator animator;
    BoxCollider boxCollider;

    //patrol
    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;

    //state change
    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;


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
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        if (!playerInSight && !playerInAttackRange) Patrol();
        if (playerInSight && !playerInAttackRange) Chase();
        if (playerInSight && playerInAttackRange) Attack();
    }

    void Chase()
    {   
        agent.SetDestination(player.transform.position);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            animator.SetTrigger("Chase");
            agent.SetDestination(player.transform.position);
        }
    }

    void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("swiping"))
        {
            animator.SetTrigger("Attack");
            agent.SetDestination(transform.position); 
        }
    }

    void Patrol()
    {
        if (!walkpointSet) SearchForDest();
        if (walkpointSet)
        {
            agent.SetDestination(destPoint);
            animator.SetTrigger("Patrol");
        } 
        if (Vector3.Distance(transform.position, destPoint) < 10) walkpointSet = false;
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet = true;
        }
    }

    void EnableAttack ()
    {
        boxCollider.enabled = true;
    }

    void DisableAttack ()
    {
        boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<playerMovement>();
        if (player != null)
        {
            print("HIT");
        }
    }

}
