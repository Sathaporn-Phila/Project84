/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealh : EnemyHealth
{
    public RemoveWall removeWall;
    
    private void Start() {
        animator = GetComponent<Animator>();
    }
    public override void die()
    {
        if (HP <= 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("dying"))
            {
                animator.SetTrigger("Dying"); 
                removeWall.remove();
            }
        }
    }
}*/
