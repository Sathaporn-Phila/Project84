using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        transform.LookAt(player);
    }
}
