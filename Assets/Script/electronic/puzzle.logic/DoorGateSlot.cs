using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class DoorGateSlot : DoorSlot
{
    public override void setSlot()
    {
        query = this.gameObject.AddComponent<Query>();
        foreach(GameObject collider in query.queryByName(this.transform.parent.parent.gameObject,new Regex(@"\bcollider"))){
            allSlot.Add(collider.GetComponent<GateSlot>());
        }
    }
    private void Update() {
        Ray ray = new Ray(transform.position,transform.TransformDirection(Vector3.forward));
        currerntState.UpdateState(this,skinnedMesh,wireQuery.findWireHit(ray,2,0));


    }
}
