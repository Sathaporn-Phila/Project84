using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class normalGen : wireProp
{
    
    List<MeshRenderer> childRenderer = new List<MeshRenderer>();
    float baseVolt = 5;
    wireQuery wireQueryGroup;
    doorWeaponSlot weaponSlot;
    private void Awake() {
        voltage = Random.Range(0,2)*baseVolt;
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
        foreach(MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>().Where(obj=>obj.gameObject.name!=this.gameObject.name)){
            wireQueryGroup.SetColor(voltage,meshRenderer);
            childRenderer.Add(meshRenderer);
        }
        weaponSlot = this.transform.parent.parent.Find("puzzle.unlock/wire.slot.withDoor").GetComponent<doorWeaponSlot>();
        if(weaponSlot){
            weaponSlot.allgen.Add(this);
        }
    }
    public void reset(){
        voltage = Random.Range(0,2)*baseVolt;
    }

    // Update is called once per frame
    
}
