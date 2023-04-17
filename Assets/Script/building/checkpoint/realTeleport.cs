using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realTeleport : teleportPoint
{
    public GameObject destPoint;
    
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<PlayerHealth>(out PlayerHealth player)){
            player.playerInfo.transform.position = destPoint.transform.position;
        }
    }
    
}
