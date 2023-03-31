using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class DoorGateSlot : DoorSlot
{
    public override void Awake() {
        query = this.gameObject.AddComponent<Query>();
        foreach(GameObject collider in query.queryByName(this.transform.parent.parent.gameObject,new Regex(@"\bcollider"))){
            allSlot.Add(collider.GetComponent<GateSlot>());
        }
        
    }
}
