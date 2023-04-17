using UnityEngine;
using System.Linq;
public class doorRoom : MonoBehaviour {
    //Start is called before the first frame update
    enum doorInitialState{
        Close,Open
    }
    public Animator m_animator;
    public doorState currentState;
    public doorAnimOpen doorOpen;
    public doorAnimClose doorClose;
    public AudioSource audiosound;
    public AudioClip opensound;
    public AudioClip closesound;

    [SerializeField] doorInitialState doorInitial = new();
    public virtual void Awake() {
        setData();
        currentState = doorInitial == doorInitialState.Close ? doorClose : doorOpen;
        currentState.Enter(m_animator);
    }
    public void setData(){
        doorOpen = this.gameObject.AddComponent<doorAnimOpen>();
        doorClose = this.gameObject.AddComponent<doorAnimClose>();
        m_animator = this.gameObject.GetComponent<Animator>();
    }
    public void Open(){
        currentState = doorOpen;
        currentState.Enter(m_animator);
        audiosound.PlayOneShot(opensound);
    }
    public void Close(){
        currentState = doorClose;
        currentState.Enter(m_animator);
        audiosound.PlayOneShot(closesound);
    }    
}
