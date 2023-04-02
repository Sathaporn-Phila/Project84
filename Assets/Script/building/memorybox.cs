using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR.Interaction.Toolkit;

[ExecuteAlways]
public class memorybox : MonoBehaviour
{
    MeshFilter meshFilter;
    GameObject fresnelObj,centerVFX;
    memoryPlatform memoryPlatform;
    MaterialPropertyBlock mpb;
    public Transform formerParent;
    public MaterialPropertyBlock Mpb {
        get {
            mpb = mpb == null? new MaterialPropertyBlock():mpb;
            return mpb;
        }
    }
    [ColorUsage(false, true)] public Color fresnelColor,subcolor = new();
    VisualEffect effect;
    bool isAnimated = false;
    public enum chooseMesh {
        resistor,diode,gate
    }
    public chooseMesh selectMesh = new();
    private void Start() {
        fresnelObj = this.transform.Find("memory").gameObject;
        this.gameObject.GetComponent<XRGrabInteractable>().interactionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        effect = this.transform.Find("memory").GetComponent<VisualEffect>();
    
        centerVFX = GameObject.Find("platform/Sphere").gameObject;
        memoryPlatform = GameObject.Find("platform").GetComponent<memoryPlatform>();
        meshFilter = fresnelObj.GetComponent<MeshFilter>();
        formerParent = this.transform.parent;
        MeshRenderer renderer = fresnelObj.GetComponent<MeshRenderer>();

        if(selectMesh == chooseMesh.resistor){
            meshFilter.mesh = Resources.Load<GameObject>("Prefabs/electronic/resistor").GetComponent<MeshFilter>().sharedMesh;
            fresnelObj.transform.localScale = Vector3.one*0.2f;
            
        }else if(selectMesh == chooseMesh.diode){
            meshFilter.mesh = Resources.Load<GameObject>("Prefabs/electronic/diode").GetComponent<MeshFilter>().sharedMesh;
            fresnelObj.transform.localScale = Vector3.one;
            
        }else if(selectMesh == chooseMesh.gate){
            meshFilter.mesh = Resources.Load<GameObject>("Prefabs/electronic/gate.machine.module/gate.and").GetComponent<MeshFilter>().sharedMesh;
            fresnelObj.transform.localScale = Vector3.one*0.5f;
        }

        //PlayEvent("StopSphere");

        Mpb.SetColor("_coatColor",fresnelColor);
        Mpb.SetColor("_coatSubColor",subcolor);
        renderer.SetPropertyBlock(Mpb);
    }
    
    public void Animate(){
        if(!isAnimated){
            PlayBezier();
        }else{
            PlaySphere();
        }
        isAnimated = !isAnimated;
    }
    void PlayBezier(){
        PlayEvent("StopSphere");
        PlayEvent("PlayBezier");
    }
    void PlaySphere(){
        PlayEvent("StopSphere");
        PlayEvent("PlayBezier");
    }

    void PlayEvent(string nameEvent){
        effect.SendEvent(nameEvent);
    }
    public void SetBezierPoint(){
        Vector3 dist = transform.InverseTransformDirection(centerVFX.transform.position-this.transform.position)*(1/fresnelObj.transform.localScale.x);
        effect.SetVector3("objFinalPos",dist);
        effect.SetVector4("ParticleColor",fresnelColor);
    }
}
