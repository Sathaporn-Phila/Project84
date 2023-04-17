using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
public class wireDiodeSlot : wireProp
{
    // Start is called before the first frames update
    float scale;
    public float waitCounter;
    public toggleRay toggleRay;
    WaveGenerator waveGenerator;
    public diodeSlot diodeSlot;
    wireQuery wireQueryGroup;
    
    
    private void Awake() {
        toggleRay = new toggleRay(new Ray(transform.position,transform.TransformDirection(-1,0,-1)),0);
        //Debug.DrawRay(toggleRay.m_ray.origin,toggleRay.m_ray.direction,Color.blue,60);
        Mesh mesh = this.gameObject.transform.Find("wire.slot.diode").GetComponent<MeshFilter>().mesh;
        scale = Vector2.Distance(Vector2.zero,new Vector2(mesh.bounds.size.x,mesh.bounds.size.z));
        waveGenerator = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
        diodeSlot = this.gameObject.transform.Find("collider").gameObject.GetComponent<diodeSlot>();
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
    }

    void FixedUpdate(){
        float cosValue = waveGenerator.getCosValue();
        
        if(diodeSlot.collideGameObject){
            Ray nextHitRay = toggleRay.getRay();
            nextHitRay.direction = -nextHitRay.direction;
            GameObject hitLeftObj = wireQueryGroup.findParentObjectHit(toggleRay.getRay(),scale,7);
            GameObject hitRightObj = wireQueryGroup.findParentObjectHit(nextHitRay,scale,7);

            if(hitLeftObj && hitRightObj){
                wireY wireYLeft = hitLeftObj.GetComponent<wireY>();
                wireY wireYright = hitRightObj.GetComponent<wireY>();
                if(wireYLeft.voltage < 0 && wireYright.prevVolt > 0 && wireYright.voltage == 0){
                    waitCounter = 17;//num n object to wait;
                }else{
                    waitCounter--;
                }
                //ป้องกันการเกิดวนลูป
                if(waitCounter<=0){
                    voltage = wireYLeft.voltage*wireYright.voltage<0?0:wireYLeft.voltage;
                }else{
                    voltage = 0;
                }
            }
        
        }else{
            voltage = 0;
        }
    }
}
