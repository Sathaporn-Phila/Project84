using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class locker : MonoBehaviour
{
    private AnimationClip[] AnimClip;
    private List<AnimationClip> animList;
    private AnimatorOverrideController controller;
    doorRoom DoorState; 
    private Animator m_animator;
    // Start is called before the first frame update
    void Awake()
    {
        DoorState = GameObject.Find("elec.door").GetComponent<doorRoom>();        
        m_animator = GetComponent<Animator>();
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}    
