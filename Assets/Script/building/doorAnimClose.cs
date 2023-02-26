using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
    public override void UpdateState(safeBoxDoor safeBoxDoor)
    {
        //Debug.Log(safeBoxDoor.Mpb.GetFloatArray("_IntArray"));
    }
    public override void UpdateState(safeBoxDoor safeBoxDoor,string input)
    {
        if(input == "OK"){
            if(safeBoxDoor.safeboxPassword.get() == safeBoxDoor.safeboxPassword.current){
                safeBoxDoor.changeState(safeBoxDoor.doorOpen);
                
            }
        }else{
            safeBoxDoor.safeboxPassword.append(input);
        }
        float n;
        List<float> binaryInput = safeBoxDoor.safeboxPassword.current.Select(c => float.TryParse(c.ToString(), out n) ? n : 0).ToList();
        List<float> emptyInput = Enumerable.Repeat(-1f,8-binaryInput.Count).ToList();
        binaryInput.AddRange(emptyInput);
        safeBoxDoor.Mpb.SetFloatArray("_IntArray",binaryInput);
        safeBoxDoor.meshRenderer.SetPropertyBlock(safeBoxDoor.Mpb);
        Debug.Log(string.Join(",",safeBoxDoor.Mpb.GetFloatArray("_IntArray")));
    }
}
