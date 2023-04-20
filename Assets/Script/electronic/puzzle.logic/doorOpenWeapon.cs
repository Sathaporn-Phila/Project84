using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpenWeapon : doorAnimOpen
{
    // Start is called before the first frame update
    EnemyHealth enemyHealth;
    private void Awake() {
        enemyHealth = GameObject.FindGameObjectWithTag("boss").GetComponent<EnemyHealth>();
    }
    public override void UpdateState(DoorSlot doorSlot,SkinnedMeshRenderer skinnedMesh,float voltage){
        Vector3 relativePos = transform.InverseTransformDirection(this.transform.position-enemyHealth.transform.position);
        if(voltage == 5){
            this.animateDoorOpen(doorSlot,skinnedMesh);
        }else{
            doorSlot.changeState(doorSlot.doorClose);
        }
    }
    public void animateDoorOpen(DoorSlot doorSlot,SkinnedMeshRenderer skinnedMesh){
        float blendShape = skinnedMesh.GetBlendShapeWeight(0);
        float blendShapeSpeed = 0.5f;
        if(skinnedMesh.GetBlendShapeWeight(0)>=0){
            skinnedMesh.SetBlendShapeWeight(0,blendShape-blendShapeSpeed);
            if(skinnedMesh.GetBlendShapeWeight(0)==0){
                Debug.Log("here");
                this.action(doorSlot);
            }
        }
    }
    public override void action(DoorSlot doorSlot){
        Debug.Log("here2");
        enemyHealth.HP -= 10;
        doorSlot.vfx.SendEvent("PlayLaserBeam");
        doorSlot.laserBeam.Play();
        this.transform.parent.parent.parent.Find("box").GetComponent<Box>().respawn();
        doorSlot.reset();
    }

}
