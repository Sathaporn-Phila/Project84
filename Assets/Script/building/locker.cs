using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class locker : MonoBehaviour
{
    private AnimationClip[] AnimClip;
    private List<AnimationClip> animList;
    private AnimatorOverrideController controller;
    private doorState DoorState; 
    private Animator m_animator;
    // Start is called before the first frame update
    void Awake()
    {
        DoorState = GameObject.Find("elec.door").GetComponent<doorState>();        
        m_animator = GetComponent<Animator>();
        AnimClip = Resources.LoadAll("Animation/door/"+this.GetType().Name,typeof(AnimationClip)).Cast<AnimationClip>().ToArray();
        animList = new List<AnimationClip>(AnimClip);
        changeClip();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    private void changeClip(){
        controller = new AnimatorOverrideController(DoorState.getAnimController);
        m_animator.runtimeAnimatorController = controller;
        controller["elec.idle"] = animList.Find(anim => anim.name == "locker.idle");
    }
}
