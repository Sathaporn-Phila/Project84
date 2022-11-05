using UnityEngine;

public class doorState : MonoBehaviour
{
    // Start is called before the first frame update
    Animator m_animator;
    private void Awake() {
        m_animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        m_animator.SetInteger("doorState",1);
    }
    public RuntimeAnimatorController getAnimController{
        get {
            return GetComponent<Animator>().runtimeAnimatorController;
        }
    }
}
