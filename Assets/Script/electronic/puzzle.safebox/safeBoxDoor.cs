using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class safeBoxDoor : MonoBehaviour
{
    // Start is called before the first frame update
    
    Animator m_animator;
    
    public MeshRenderer meshRenderer;
    doorState currentState;
    public doorState CurrentState{
        get{return currentState;}
    }
    public doorAnimOpen doorOpen;
    public doorAnimClose doorClose;
    public doorState getCurrentState(){
        return currentState;
    }
    public class Password {
        private string Value = "";
        public string current = "";
        public Password(){
            randomPassword();
            //Debug.Log(Value);
        }
        public string get(){
            return Value;
        }
        public void append(string value){
            Debug.Log(current.Length);
            if( value != "DEL" &&current.Length<8){
                if(value == 0.ToString() || value == 1.ToString()){
                    current += value;
            }}else if(value == "DEL" && current.Length > 0){
                    current = current.Substring(0,current.Length-1);
            }
        }
        public void randomPassword(){
            string rnd = "";
            for(int i=0;i<8;i++){
                rnd += Random.Range(0,2).ToString();
            }
            Debug.Log(current.Length);
            Value = rnd;
        }
    }
    public Password safeboxPassword;
    MaterialPropertyBlock mpb;

    public MaterialPropertyBlock Mpb {
        get {
            mpb = mpb == null? new MaterialPropertyBlock():mpb;
            return mpb;
        }
    }
    public void changeState(doorState state){
        currentState = state;
        currentState.Enter(m_animator);
    }
    
    private void SetAnim(){
       
        m_animator = GameObject.Find("safeBox").GetComponent<Animator>();
        List<AnimationClip> animList = Resources.LoadAll<AnimationClip>("Animation/door/safebox").ToList();
        AnimatorOverrideController controller = new AnimatorOverrideController(m_animator.runtimeAnimatorController);
        m_animator.runtimeAnimatorController = controller;
        foreach(var item in controller.animationClips){
            controller[item.name] = animList.Find(anim => anim.name == item.name);
        }
            
    }
    private void Awake() {
        safeboxPassword = new Password();
        doorOpen = this.gameObject.AddComponent<doorAnimOpen>();
        doorClose = this.gameObject.AddComponent<doorAnimClose>();
        meshRenderer = GetComponent<MeshRenderer>();
        SetAnim();
        Mpb.SetFloatArray("_IntArray",new List<float>(){-1f,-1f,-1f,-1f,-1f,-1f,-1f,-1f});
        meshRenderer.SetPropertyBlock(Mpb);
        currentState = doorClose;
        currentState.Enter(m_animator);
    }
    // Update is called once per frame
    public virtual void Update() {
        currentState.UpdateState(this);
    }
    public virtual void UpdateState(string input){}
}
