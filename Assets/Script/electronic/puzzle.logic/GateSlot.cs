using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GateSlot : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject orGate;
    wireQuery wireQueryGroup;
    DoorSlot slot;
    
    private void Awake() {
        orGate = (GameObject)Resources.Load("Prefabs/electronic/gate.machine.module/gate.nand");
        orGate.transform.localScale = Vector3.one*0.95f;
        Instantiate(orGate,this.transform.position+3*Vector3.up,Quaternion.Euler(0,180,0));
        slot = this.transform.root.Find("puzzle.gate.machine/puzzle.unlock/wire.slot.withDoor").GetComponent<DoorSlot>();     
    }
    private void hitObject(){}
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<Gate>(out Gate gateInput)){
            gateInput.isInSlot += gateInput.getVoltageFromHit;
            if(!this.slot.triggerSlots.Contains(this)){
                slot.triggerSlots.Add(this);
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent<Gate>(out Gate gateInput)){
            gateInput.isInSlot -= gateInput.getVoltageFromHit;
            if(this.slot.triggerSlots.Contains(this)){
                slot.triggerSlots.Remove(this);
            }
        }
    }
}