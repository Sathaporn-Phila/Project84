using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;

public class DoorSlot : MonoBehaviour {
    public List<GateSlot> triggerSlots = new List<GateSlot>(),allSlot = new List<GateSlot>();
    doorState currerntState;
    public doorAnimOpen doorOpen;
    public doorAnimClose doorClose;
    Query query;
    wireQuery wireQuery;
    SkinnedMeshRenderer skinnedMesh;
    private void Awake() {
        doorOpen = new doorAnimOpen();
        doorClose = new doorAnimClose();
        query = this.gameObject.AddComponent<Query>();
        wireQuery = this.gameObject.AddComponent<wireQuery>();
        skinnedMesh = this.gameObject.GetComponent<SkinnedMeshRenderer>();
        
        foreach(GameObject collider in query.queryByName(this.transform.root.gameObject,new Regex(@"\bcollider"))){
            allSlot.Add(collider.GetComponent<GateSlot>());
        }
        currerntState = doorClose;
    }
    private void Update() {
        Ray ray = new Ray(transform.position,transform.TransformDirection(Vector3.forward));
        
        if(allSlot.Count == triggerSlots.Count){
            currerntState.UpdateState(this,skinnedMesh,wireQuery.findWireHit(ray,2,0));
        }

    }
    public void changeState(doorState state){
        currerntState = state;
    }
}
