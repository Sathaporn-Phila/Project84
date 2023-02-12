using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Gate : MonoBehaviour
{
   public float voltage,baseVoltage = 5,voltageLeft,voltageRight;
   public event Action isInSlot;
   public abstract float calVoltage(float input1,float input2);
   public Ray rayInputLeft,rayInputRight;
   wireQuery wireQueryGroup;
   public virtual float getVoltage(){
      return voltage;
   }
   public void getVoltageFromHit(){
      //Debug.Log(wireQueryGroup.findParentObjectHit(rayInputLeft,2,0));
      voltageLeft = wireQueryGroup.findWireHit(rayInputLeft,2);
      voltageRight = wireQueryGroup.findWireHit(rayInputRight,2);
   }

   private void Awake() {
      wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
   }

   private void FixedUpdate() {
      rayInputLeft = new Ray(transform.position+transform.TransformDirection(new Vector3((float)0.5,0,1)),transform.TransformDirection(Vector3.forward));
      rayInputRight = new Ray(transform.position+transform.TransformDirection(new Vector3((float)-0.5,0,1)),transform.TransformDirection(Vector3.forward));
   
      isInSlot?.Invoke();
      if(isInSlot != null){
         voltage = calVoltage(voltageLeft,voltageRight);
      }
   }
}
