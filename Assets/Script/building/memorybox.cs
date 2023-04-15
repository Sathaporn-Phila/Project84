using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR.Interaction.Toolkit;
using Realms;
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
    MeshRenderer meshRenderer;
    GradientEffect gradientEffect; 
    MemoryBoxData memoryBoxData;
    bool isAnimated = false;
    public enum chooseMesh {
        resistor,diode,gate
    }
    public chooseMesh selectMesh = new();
    private Realm _realm;
    public virtual void Start() {
        Application.wantsToQuit += WanttoQuit;
        _realm = Realm.GetInstance();
        memoryBoxData = _realm.Find<MemoryBoxData>(this.FindPath(this.transform));
        if(memoryBoxData is null){
            _realm.Write(()=>{
                memoryBoxData = _realm.Add(new MemoryBoxData(this.FindPath(this.transform), this.transform));
            });
        }else{
            this.transform.position = memoryBoxData.transformModel.Position;
        }

        fresnelObj = this.transform.Find("memory").gameObject;
        this.gameObject.GetComponent<XRGrabInteractable>().interactionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        effect = fresnelObj.GetComponent<VisualEffect>();
    
        centerVFX = GameObject.Find("platform/Sphere");
        
        memoryPlatform = GameObject.Find("platform").GetComponent<memoryPlatform>();
        meshFilter = fresnelObj.GetComponent<MeshFilter>();
        formerParent = this.transform.parent;

        this.setScaleFresnelObj();
        this.setFresnelColor();
    }

    void setScaleFresnelObj(){
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
    }
    void setFresnelColor(){
        meshRenderer = fresnelObj.GetComponent<MeshRenderer>();
        Mpb.SetColor("_coatColor",fresnelColor);
        Mpb.SetColor("_coatSubColor",subcolor);
        meshRenderer.SetPropertyBlock(Mpb);
        gradientEffect = new GradientEffect(fresnelColor,subcolor);

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
        //Vector3 dist = transform.InverseTransformDirection(centerVFX.transform.position-this.transform.position)*(1/fresnelObj.transform.localScale.x);

        //change from world space to local space
        Vector3 relativePos = transform.InverseTransformDirection(this.transform.position-centerVFX.transform.position);
        float dist = relativePos.magnitude;
        //Debug.Log(relativePos);
        Quaternion rot = Quaternion.LookRotation(relativePos,Vector3.up)*Quaternion.Euler(0,180,0);
        //Debug.Log(rot.eulerAngles);
        //Debug.Log(string.Join(":",relativePos,rot.eulerAngles));
        effect.SetFloat("LineScale",dist);
        effect.SetVector3("LineRotation",rot.eulerAngles);
        effect.SetGradient("Gradient",gradientEffect.gradient);
    }
    string FindPath(Transform t){
        string path = t.name;

        while (t.parent != null) {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
    private void saveObjectPosition(){
        /*_realm.Write(()=>{
            memoryBoxData.transformModel.Position = this.transform.position;
            memoryBoxData.transformModel.Rotation = this.transform.rotation;
        });*/
    }
    bool WanttoQuit(){
        this.saveObjectPosition();
        return true;
    }
}
