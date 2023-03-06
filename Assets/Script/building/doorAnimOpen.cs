using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAnimOpen : doorState
{
    // Start is called before the first frame update
    public override void Enter(Animator m_animator)
    {
        m_animator.SetInteger("doorState",1);
    }
    
    public void UpdateState(doorRoom doorRoom,Animation m_Animator){
    }

    public void UpdateState(DoorSlot doorSlot,SkinnedMeshRenderer skinnedMesh){
    }
    public override void UpdateState(DoorSlot doorSlot,SkinnedMeshRenderer skinnedMesh,float voltage){
        if(voltage == 5){
            float blendShape = skinnedMesh.GetBlendShapeWeight(0);
            float blendShapeSpeed = 0.5f;
            if(skinnedMesh.GetBlendShapeWeight(0)>0){
                skinnedMesh.SetBlendShapeWeight(0,blendShape-blendShapeSpeed);
            }
        }else{
            doorSlot.changeState(doorSlot.doorClose);
        }
    }
    
}
