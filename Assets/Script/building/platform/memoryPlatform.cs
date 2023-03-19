using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class memoryPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    List<memorybox> memoryboxes = new();
    [SerializeField] ComputeShader _compute = null;
    
    VisualEffect centerVFX;
    MeshFilter mesh;
    float childCount;
    public float scale;
    private void Awake() {
        mesh = GetComponent<MeshFilter>();
        scale = mesh.sharedMesh.bounds.size.x;
        centerVFX = this.transform.Find("Sphere").GetComponent<VisualEffect>();
        childCount = this.gameObject.GetComponentsInChildren<platformSlot>().Length;            
    }
    public void AddMembox(memorybox membox){
        memoryboxes.Add(membox);
        Debug.Log(memoryboxes.Count);
        if(memoryboxes.Count == childCount){
            Animate();
        }
    }
    public void Animate(){
        foreach(memorybox membox in memoryboxes){
            membox.Animate();
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
