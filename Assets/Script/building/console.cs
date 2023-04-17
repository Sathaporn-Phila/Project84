using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class console : MonoBehaviour
{
    public card rfidCard;
    private void Awake() {
        rfidCard = GameObject.Find("card").GetComponent<card>();
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject == rfidCard.gameObject){
            this.transform.parent.Find("Wall_Door_02").GetComponent<doorRoom>().Open();
        }
    }
}
