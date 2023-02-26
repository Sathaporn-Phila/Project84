using UnityEngine;

public class doorRoom : MonoBehaviour {
    //Start is called before the first frame update
    Animator m_animator;
    doorState currentState;
    doorAnimOpen doorOpen;
    doorAnimClose doorClose;
    private void Awake() {
        doorOpen = this.gameObject.AddComponent<doorAnimOpen>();
        doorClose = this.gameObject.AddComponent<doorAnimClose>();
        m_animator = this.gameObject.GetComponent<Animator>();
        currentState = doorOpen;
        currentState.Enter(m_animator);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
