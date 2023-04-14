using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBarrierAttack : MonoBehaviour
{
    [SerializeField] float damage;

    BoxCollider triggerBox;

    private void Start()
    {
        triggerBox = GetComponent<MeshCollider>();
        triggerBox.enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.HP -= damage;
            if (enemy.HP <= 0)
            {
                enemy.die();
            }

        }
    }
}
