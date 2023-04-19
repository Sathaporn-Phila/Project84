using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{   
    public Animator animator;

    public float HP = 100;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update() 
    {
        die();
    }
    
    public virtual void die()
    {
        if (HP <= 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("dying"))
            {
                animator.SetTrigger("Dying"); 
            }
        }
    }
}
