using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateEnemyObject : MonoBehaviour
{
    public GameObject enemy;
    public BoxCollider player;
    public GameObject activateDoor;

    public ActivateEnemyObject activator;

    CapsuleCollider teleport;

    public bool toggleDeactive;

    void Start()
    {
        teleport = GetComponent<CapsuleCollider>();
        player = GameObject.Find("Robot Kyle").GetComponent<BoxCollider>();
        activator = activateDoor.GetComponent<ActivateEnemyObject>();
    }

    /*void Update () 
    {
        if (toggleDeactive)
        {
            if (enemy.activeSelf)
            {
                enemy.SetActive(false);
                activator.toggleActive = false;
            }
        }
    }*/

    public void OnTriggerEnter(Collider other)
    {
        if (enemy.activeSelf)
        {
            enemy.SetActive(false);
        }
    }
}
