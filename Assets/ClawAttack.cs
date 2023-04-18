using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : MonoBehaviour
{
    [SerializeField] float damage;
    
    Animator animator;
    BoxCollider triggerBox;

    private void Start()
    {
        triggerBox = GetComponent<BoxCollider>();
        animator = GetComponentInParent<Animator>();
    }

    public void EnableEnemyAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("swiping"))
        {
            triggerBox.enabled = true;
        }
        else
        {
            triggerBox.enabled = false;
        }
        
    }

    /*public void EnableEnemyAttack()
    {
        triggerBox.enabled = true;
    }

    public void DisableEnemyAttack()
    {
        triggerBox.enabled = false;
    }*/

    public void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerHealth>();
        var sound = GetComponent<SoundController>();
        if (player != null)
        {
            player.HP -= damage;
            sound.hitSound();
            if (player.HP <= 0)
            {
                player.die();
            }

        }
    }
}

