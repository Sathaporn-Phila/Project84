using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : MonoBehaviour
{
    [SerializeField] float damage;

    BoxCollider triggerBox;

    private void Start()
    {
        triggerBox = GetComponent<BoxCollider>();
        triggerBox.enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerHealth>();
        var sound = GetComponent<SoundController>();
        if (player != null)
        {
            player.HP -= damage;
            if (player.HP <= 0)
            {
                player.die();
                sound.hitSound();
            }

        }
    }
}

