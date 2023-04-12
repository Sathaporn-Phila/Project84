using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpenWeapon : doorAnimOpen
{
    // Start is called before the first frame update
    public override void animateDoorOpen(SkinnedMeshRenderer skinnedMesh){
        float blendShape = skinnedMesh.GetBlendShapeWeight(0);
        float blendShapeSpeed = 0.5f;
        if(skinnedMesh.GetBlendShapeWeight(0)>0){
            skinnedMesh.SetBlendShapeWeight(0,blendShape-blendShapeSpeed);
            if(skinnedMesh.GetBlendShapeWeight(0)==0){
                action();
            }
        }
    }
    public void action(){
        ////boss.hp -= 1;
        this.transform.parent.parent.parent.Find("Box").GetComponent<Box>().respawn();
    }

}
