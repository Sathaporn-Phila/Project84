using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realTeleport : teleportPoint
{
    public GameObject destPoint;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Is trigger");
        if(other.TryGetComponent<playerDataController>(out playerDataController player)){
            Debug.Log("Can player trigger");
            player.transform.position = destPoint.transform.position;
        }
    }

}
