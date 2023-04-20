using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
public class checkpointData : RealmObject {
    [PrimaryKey]
    public string path{get;set;}
    public TransformModel transformModel{get;set;}
    public bool isTouched{get;set;}
    public checkpointData(string name,Transform transform){
        path = name;
        transformModel = new TransformModel(){
            Position = transform.position,Rotation = transform.rotation,Scale = transform.localScale
        };
        isTouched = false;
    }
    public checkpointData(){}
}
public class checkPointState {
    public virtual void Enter(SkinnedMeshRenderer skinnedMesh){
        skinnedMesh.SetBlendShapeWeight(0,0);
    }
    public virtual void UpdateState(checkpoint checkpoint,SkinnedMeshRenderer skinnedMesh){}
}
public class idleCheckpoint : checkPointState{
}
public class TouchCheckpoint : checkPointState{
    public override void Enter(SkinnedMeshRenderer skinnedMesh){
        skinnedMesh.SetBlendShapeWeight(0,100);
    }
    public override void UpdateState(checkpoint checkpoint,SkinnedMeshRenderer skinnedMesh){
        float weight = skinnedMesh.GetBlendShapeWeight(0);
        if(weight<100){
            skinnedMesh.SetBlendShapeWeight(0,weight+2);
        }else{
            checkpoint.setIsTouched();
        }
    }
}
public class checkpoint : MonoBehaviour
{
    [SerializeField] public string checkpointName;
    private Realm _realm;
    checkpointData checkpointinfo;
    checkPointState state;
    idleCheckpoint idle = new();
    TouchCheckpoint touchCheckpoint = new();
    SkinnedMeshRenderer skinnedMeshRenderer;
    public AudioSource audiosound;
    public AudioClip checkpointsound;
    private void Awake() {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        _realm = Realm.GetInstance();
        checkpointinfo = _realm.Find<checkpointData>(checkpointName);
        if(checkpointinfo is null){
            _realm.Write(()=>{
                checkpointinfo = _realm.Add(new checkpointData(checkpointName, this.transform.Find("Cube").transform));
            });
        }
        if(checkpointinfo.isTouched){
            state = touchCheckpoint;
            state.Enter(skinnedMeshRenderer);
        }else{
            state = idle;
            state.Enter(skinnedMeshRenderer);
        }
    }
    private void OnTriggerEnter(Collider other) {
        audiosound.PlayOneShot(checkpointsound);
        state = touchCheckpoint;
        Debug.Log("checkpoint trigger");
        GameObject.Find("Robot Kyle").GetComponent<PlayerHealth>().playerInfo.setCurrentCheckpoint(checkpointName);
        
    }
    public void setIsTouched(){
        _realm.Write(()=>{
            checkpointinfo.isTouched = true;
        });
    }
    private void Update() {
        state.UpdateState(this,skinnedMeshRenderer);
    }
}
