using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.VFX;



public class DoorSlot : MonoBehaviour {
    public List<GateSlot> triggerSlots = new List<GateSlot>(),allSlot = new List<GateSlot>();
    public doorState currerntState;
    public doorAnimOpen doorOpen;
    public doorAnimClose doorClose;
    public Query query;
    public wireQuery wireQuery;
    public SkinnedMeshRenderer skinnedMesh;

    public VisualEffect vfx;
    public AudioSource laserBeam;
    public virtual void Awake() {
        this.setInitValue();
        this.setSlot();
    }
    public virtual void setInitValue(){
        doorOpen = this.gameObject.AddComponent<doorAnimOpen>();
        doorClose = this.gameObject.AddComponent<doorAnimClose>();
        wireQuery = this.gameObject.AddComponent<wireQuery>();
        skinnedMesh = this.gameObject.GetComponent<SkinnedMeshRenderer>();    
        currerntState = doorClose;
        currerntState.Enter(this.skinnedMesh);
    }
    public virtual void setSlot(){}
    
    public virtual void changeState(doorState state){
        currerntState = state;
    }
    public virtual void reset(){}
}
