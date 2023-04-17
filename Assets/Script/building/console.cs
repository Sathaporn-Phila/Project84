using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class console : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.TryGetComponent<card>(out card owner)){
            this.transform.parent.Find("Wall_Door_02").GetComponent<doorRoom>().Open();
        }
    }
}
