using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectStateMachine
{
    public virtual void Enter(Animator m_animator){}
    public virtual void UpdateState(){}
}
public class idleEffect : EffectStateMachine
{
    public override void Enter(Animator m_animator){
        m_animator.SetInteger("stateType",0);
    }
    public override void UpdateState(){}
}
public class tornadoEffect : EffectStateMachine
{
    public override void Enter(Animator m_animator){
        m_animator.SetInteger("stateType",1);
    }
    public override void UpdateState(){}
}