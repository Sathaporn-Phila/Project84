using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float HP = 100;
    public playerDataController playerInfo; 
    private void Start() {
        playerInfo = GameObject.Find("VR/XR Origin").GetComponent<playerDataController>();
        //die();
    }
    
    public void die()
    {
        playerInfo.transform.position = playerInfo.getPostionfromCheckpoint();
        HP = 100;
    }
}
