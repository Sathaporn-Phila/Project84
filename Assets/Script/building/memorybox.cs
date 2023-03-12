using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class memorybox : MonoBehaviour
{
    MeshFilter meshFilter;
    GameObject fresnelObj;
    MaterialPropertyBlock mpb;
    public MaterialPropertyBlock Mpb {
        get {
            mpb = mpb == null? new MaterialPropertyBlock():mpb;
            return mpb;
        }
    }
    [ColorUsage(false, true)] public Color fresnelColor,subcolor = new();
    public enum chooseMesh {
        resistor,diode,gate
    }
    public chooseMesh selectMesh = new();
    private void Start() {
        fresnelObj = this.transform.Find("memory").gameObject;
        meshFilter = fresnelObj.GetComponent<MeshFilter>();
        MeshRenderer renderer = fresnelObj.GetComponent<MeshRenderer>();
        if(selectMesh == chooseMesh.resistor){
            meshFilter.mesh = Resources.Load<GameObject>("Prefabs/electronic/resistor").GetComponent<MeshFilter>().sharedMesh;
            fresnelObj.transform.localScale = Vector3.one*0.7f;
            
        }else if(selectMesh == chooseMesh.diode){
            meshFilter.mesh = Resources.Load<GameObject>("Prefabs/electronic/diode").GetComponent<MeshFilter>().sharedMesh;
            fresnelObj.transform.localScale = Vector3.one;
            
        }else if(selectMesh == chooseMesh.gate){
            meshFilter.mesh = Resources.Load<GameObject>("Prefabs/electronic/gate.machine.module/gate.and").GetComponent<MeshFilter>().sharedMesh;
            fresnelObj.transform.localScale = Vector3.one*0.5f;
        }
        Mpb.SetColor("_coatColor",fresnelColor);
        Mpb.SetColor("_coatSubColor",subcolor);
        renderer.SetPropertyBlock(Mpb);
    }
    
}
