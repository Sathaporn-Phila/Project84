using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeBoxDoor : MonoBehaviour
{
    // Start is called before the first frame update
    
    public string passwordEnter;
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
            for(int i=0;i<8;i++){
                Value += Random.Range(0,2).ToString();
            }
        }
        public string get(){
            return Value;
        }
        public void append(string value){
            if(current.Length<8){
                if(value == 0.ToString() || value == 1.ToString()){
                    current += value;
                }else if(value == "DEL"){
                    current = current.Substring(0,current.Length-1);
                }
            }
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
    }
    
    private void Awake() {
        safeboxPassword = new Password();
        doorOpen = this.gameObject.AddComponent<doorAnimOpen>();
        doorClose = this.gameObject.AddComponent<doorAnimClose>();
        m_animator = this.gameObject.GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        Mpb.SetFloatArray("_IntArray",new List<float>(){0,0,0,0,0,0,0,0});
        currentState = doorClose;
        currentState.Enter(m_animator);
    }
    // Update is called once per frame
    private void Update() {
        currentState.UpdateState(this);
    }
}
