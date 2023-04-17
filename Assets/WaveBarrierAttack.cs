using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBarrierAttack : MonoBehaviour
{
    [SerializeField] float damage;

    MeshCollider triggerMesh;

    private void Start()
    {
        triggerMesh = GetComponent<MeshCollider>();
        triggerMesh.enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.HP -= damage;
            

        }
    }
}
