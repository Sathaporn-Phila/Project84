using UnityEngine;

public class doorRoom : MonoBehaviour {
    //Start is called before the first frame update
    Animator m_animator;
    doorState currentState;
    doorAnimOpen doorOpen = new doorAnimOpen();
    doorAnimClose doorClose = new doorAnimClose();
    private void Awake() {
        m_animator = this.gameObject.GetComponent<Animator>();
        currentState = doorOpen;
        currentState.Enter(m_animator);
    }
    // Update is called once per frame
    void Update()
    {

    }
    
}
