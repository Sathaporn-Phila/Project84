using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float HP = 100;
    /*private void Start() {
        die();
    }*/
    
    public void die()
    {
        this.transform.position = GameObject.Find("VR/XR Origin").GetComponent<playerDataController>().getPostionfromCheckpoint();
        HP = 100;
    }
}
