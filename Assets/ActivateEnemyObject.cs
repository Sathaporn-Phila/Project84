using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemyObject : MonoBehaviour
{
    public GameObject enemy;
    public BoxCollider player;

    BoxCollider door;

    public bool enemyIsActive;
    public bool toggleActive;

    void Start()
    {
        door = GetComponent<BoxCollider>();
        player = GameObject.Find("Robot Kyle").GetComponent<BoxCollider>();
        enemyIsActive = false;
        enemy.SetActive(false);
        toggleActive = false;
    }

    /*void Update () 
    {
        if (toggleActive)
        {
            enemyIsActive = true;
            enemy.SetActive(true);
        }
        else
        {
            enemyIsActive = false;
            enemy.SetActive(false);
        }
    }*/

    public void OnTriggerEnter(Collider other)
    {
        if (enemyIsActive == false)
        {
            enemyIsActive = true;
            enemy.SetActive(true);
        }
        /*else
        {
            enemyIsActive = false;
            enemy.SetActive(false);
        }*/
    }
}
