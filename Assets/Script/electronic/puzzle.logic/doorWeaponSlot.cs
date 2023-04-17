using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class doorWeaponSlot : DoorSlot
{
    public List<normalGen> allgen = new();
    
    public override void setInitValue(){
        doorOpen = this.gameObject.AddComponent<doorOpenWeapon>();
        doorClose = this.gameObject.AddComponent<doorAnimClose>();
        wireQuery = this.gameObject.AddComponent<wireQuery>();
        skinnedMesh = this.gameObject.GetComponent<SkinnedMeshRenderer>();    
        currerntState = doorClose;
        currerntState.Enter(this.skinnedMesh);
    }
    public override void setSlot()
    {
        query = this.gameObject.AddComponent<Query>();
        foreach(GameObject collider in query.queryByName(this.transform.parent.parent.gameObject,new Regex(@"\bcollider"))){
            allSlot.Add(collider.GetComponent<GateSlot>());
        }
    }
    public virtual void Update() {
        Ray ray = new Ray(transform.position,transform.TransformDirection(Vector3.forward));
        currerntState.UpdateState(this,skinnedMesh,wireQuery.findWireHit(ray,2,0));
    }
    public void reset(){
        allgen.ForEach(item=>item.reset());
    }
}
