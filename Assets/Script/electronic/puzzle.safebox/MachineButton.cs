using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineButton : Button{
    
    private void Awake() {
        safeBoxDoor = this.transform.parent.GetComponent<safeBoxDoor>();
    }
    private void OnCollisionEnter(Collision other) {
        safeBoxDoor.UpdateState(nameButton);   
    }    
}
