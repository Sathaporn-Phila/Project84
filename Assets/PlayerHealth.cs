using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float HP = 100;
    public void die(){
        transform.position = GameObject.Find("VR/XR Origin").GetComponent<playerDataController>().getPostionfromCheckpoint();
        HP = 100;
    }
}
