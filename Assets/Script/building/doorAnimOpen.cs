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

    public override void UpdateState(SkinnedMeshRenderer skinnedMesh){
        this.animateDoorOpen(skinnedMesh);
    }
    public override void UpdateState(DoorSlot doorSlot,SkinnedMeshRenderer skinnedMesh,float voltage){
        if(voltage == 5){
            this.animateDoorOpen(skinnedMesh);
        }else{
            doorSlot.changeState(doorSlot.doorClose);
        }
    }
    private void animateDoorOpen(SkinnedMeshRenderer skinnedMesh){
        float blendShape = skinnedMesh.GetBlendShapeWeight(0);
        float blendShapeSpeed = 0.5f;
        if(skinnedMesh.GetBlendShapeWeight(0)>0){
            skinnedMesh.SetBlendShapeWeight(0,blendShape-blendShapeSpeed);
        }
    }
}
