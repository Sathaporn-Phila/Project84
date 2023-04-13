using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class normalGen : wireProp
{
    
    List<MeshRenderer> childRenderer = new List<MeshRenderer>();
    float baseVolt = 5;
    wireQuery wireQueryGroup;
    private void Awake() {
        voltage = Random.Range(0,2)*baseVolt;
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
        foreach(MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>().Where(obj=>obj.gameObject.name!=this.gameObject.name)){
            wireQueryGroup.SetColor(voltage,meshRenderer);
            childRenderer.Add(meshRenderer);
        }
    }

    // Update is called once per frame
    
}