using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class doorState : MonoBehaviour
{
    public virtual void Enter(Animator m_animator){}
    public virtual void Enter(SkinnedMeshRenderer skinnedMesh){}
    public virtual void UpdateState(DoorSlot doorSlot,SkinnedMeshRenderer skinnedMesh,float voltage){
    }
    public virtual void UpdateState(safeBoxDoor safeBoxDoor){
    }
    public virtual void UpdateState(safeBoxDoor safeBoxDoor,string input){
    }
    public virtual void UpdateState(SkinnedMeshRenderer skinnedMesh){
    }
    public virtual void UpdateState(){}
    public virtual void Exit(){}
}
