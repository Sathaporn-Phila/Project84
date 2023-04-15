using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

using System.Linq;
public class memoryPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    List<memorybox> memoryboxes = new();
    [SerializeField] ComputeShader _compute = null;
    
    VisualEffect sphereEffect;
    MeshFilter mesh;
    EffectStateMachine effectState;
    idleEffect idle = new();
    tornadoEffect tornadoEffect = new();
    Animator animator;
    public GameObject teleportPoint;
    float childCount;
    bool isTornadoAnimated;
    public float scale,alpha,alphaThreshold;
    
    private void Awake() {
        mesh = GetComponent<MeshFilter>();
        scale = mesh.sharedMesh.bounds.size.x;
        sphereEffect = this.transform.Find("Sphere").GetComponent<VisualEffect>();
        animator = GetComponent<Animator>();
        childCount = this.gameObject.GetComponentsInChildren<platformSlot>().Length;
        effectState = idle;
        effectState.Enter(animator);
    }
    public void AddMembox(memorybox membox){
        memoryboxes.Add(membox);
        //Debug.Log(memoryboxes.Count);
        if(memoryboxes.Count == childCount){
            Animate();

        }
    }
    public void Animate(){
        foreach(memorybox membox in memoryboxes){
            membox.Animate();
        }
        GradientEffect gradientEffect = new(memoryboxes.Select(obj=>obj.fresnelColor).ToList());
        if(!isTornadoAnimated){
            effectState = tornadoEffect;
            //Debug.Log(sphereEffect.GetFloat("alpha"));
            sphereEffect.SetGradient("Gradient",gradientEffect.gradient);
            effectState.Enter(animator);
            isTornadoAnimated = true;
            teleportPoint.SetActive(true);
        }

    }
    public void RemoveMembox(memorybox membox){
        if(memoryboxes.Count == childCount){
            Animate();
        }
        memoryboxes.Remove(membox);
    }
    private void Update() {
        //Debug.Log(string.Join(",",data));
    }
}
