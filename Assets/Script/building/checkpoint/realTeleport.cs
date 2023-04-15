using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realTeleport : teleportPoint
{
    public GameObject destPoint;
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<playerDataController>(out playerDataController player)){
            player.transform.position = destPoint.transform.position;
        }
    }
    
}
