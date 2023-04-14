using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float HP = 100;
    public void die()
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
