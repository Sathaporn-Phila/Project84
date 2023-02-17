using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSlot : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject orGate;
    wireQuery wireQueryGroup;

    private void Awake() {
        orGate = (GameObject)Resources.Load("Prefabs/electronic/gate.machine.module/gate.or");
        Instantiate(orGate,this.transform.position,Quaternion.Euler(0,180,0));
        
    }
    private void hitObject(){}
    private void OnTriggerEnter(Collider other) {
        Debug.Log("here1");
        if(other.TryGetComponent<Gate>(out Gate gateInput)){
            gateInput.isInSlot += gateInput.getVoltageFromHit;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent<Gate>(out Gate gateInput)){
            gateInput.isInSlot -= gateInput.getVoltageFromHit;
        }
    }
}