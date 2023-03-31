using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

using System;

public class DoorSlot : MonoBehaviour {
    public List<GateSlot> triggerSlots = new List<GateSlot>(),allSlot = new List<GateSlot>();
    public doorState currerntState;
    public doorAnimOpen doorOpen;
    public doorAnimClose doorClose;
    public Query query;
    wireQuery wireQuery;
    SkinnedMeshRenderer skinnedMesh;
    public virtual void Awake() {
        doorOpen = this.gameObject.AddComponent<doorAnimOpen>();
        doorClose = this.gameObject.AddComponent<doorAnimClose>();
        wireQuery = this.gameObject.AddComponent<wireQuery>();
        skinnedMesh = this.gameObject.GetComponent<SkinnedMeshRenderer>();    
        currerntState = doorClose;
        currerntState.Enter(this.skinnedMesh);
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
