using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        GameObject player = other.transform.root.Find("XR Origin").gameObject;
        if(player){
            if(player.TryGetComponent<player>(out player me)){
                me.SetCheckpoint("checkpoint1");
            }
        }
    }
}
