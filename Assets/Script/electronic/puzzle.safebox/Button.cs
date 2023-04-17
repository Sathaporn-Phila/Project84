using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public string nameButton;
    public safeBoxDoor safeBoxDoor;
    private void Awake() {
        safeBoxDoor = this.transform.parent.Find("safeBox.door.base").GetComponent<safeBoxDoor>();
    }
    /*private void OnCollisionEnter(Collision other) {
        safeBoxDoor.CurrentState.UpdateState(safeBoxDoor,nameButton);
        Debug.Log("Button trigger");    
    }*/
    private void OnTriggerEnter(Collider other) {
        safeBoxDoor.CurrentState.UpdateState(safeBoxDoor,nameButton);
        Debug.Log("Button trigger");    
    }
}
