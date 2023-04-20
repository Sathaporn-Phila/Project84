using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{   
    //public Animator animator;
    public PatrolPath enemy;

    public float HP = 100;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        enemy = GetComponent<PatrolPath>();
    }

    private void Update() 
    {
        die();
    }
    
    public virtual void die()
    {
        if (HP <= 0)
        {
            enemy.Dying();
        }
    }
}
