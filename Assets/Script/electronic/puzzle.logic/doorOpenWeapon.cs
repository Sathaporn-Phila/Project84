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
    public void UpdateState(doorWeaponSlot doorSlot,SkinnedMeshRenderer skinnedMesh,float voltage){
        if(voltage == 5){
            this.animateDoorOpen(doorSlot,skinnedMesh);
        }else{
            doorSlot.changeState(doorSlot.doorClose);
        }
    }
    public void animateDoorOpen(doorWeaponSlot doorSlot,SkinnedMeshRenderer skinnedMesh){
        float blendShape = skinnedMesh.GetBlendShapeWeight(0);
        float blendShapeSpeed = 0.5f;
        if(skinnedMesh.GetBlendShapeWeight(0)>0){
            skinnedMesh.SetBlendShapeWeight(0,blendShape-blendShapeSpeed);
            if(skinnedMesh.GetBlendShapeWeight(0)==0){
                action(doorSlot);
            }
        }
    }
    private void action(doorWeaponSlot doorSlot){
        enemyHealth.HP -= 10;
        this.transform.parent.parent.parent.Find("Box").GetComponent<Box>().respawn();
        doorSlot.reset();
    }

}
