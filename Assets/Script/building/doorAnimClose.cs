using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAnimClose : doorState
{

    public override void Enter(Animator m_animator)
    {
        m_animator.SetInteger("doorState",0);
    }
    public override void UpdateState(DoorSlot doorSlot,SkinnedMeshRenderer skinnedMesh,float voltage){
        if(voltage == 0){
            float blendShape = skinnedMesh.GetBlendShapeWeight(0);
            float blendShapeSpeed = 2f;
            if(skinnedMesh.GetBlendShapeWeight(0)<100){
                skinnedMesh.SetBlendShapeWeight(0,blendShape+blendShapeSpeed);
            }
        }else{
            doorSlot.changeState(doorSlot.doorOpen);
        }
    }
    
}
