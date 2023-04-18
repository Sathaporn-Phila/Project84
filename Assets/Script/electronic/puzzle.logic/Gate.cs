using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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
      
      voltageLeft = wireQueryGroup.findWireHit(rayInputLeft,2,0);
      voltageRight = wireQueryGroup.findWireHit(rayInputRight,2,0);
   }

   private void Awake() {
      wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
      this.gameObject.GetComponent<XRGrabInteractable>().interactionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
   }

   private void FixedUpdate() {
      rayInputLeft = new Ray(transform.position+transform.TransformDirection(new Vector3((float)0.5,0,1)),transform.TransformDirection(Vector3.forward));
      rayInputRight = new Ray(transform.position+transform.TransformDirection(new Vector3((float)-0.5,0,1)),transform.TransformDirection(Vector3.forward));
   
      isInSlot?.Invoke();
      if(isInSlot != null){
         getVoltageFromHit();
         voltage = calVoltage(voltageLeft,voltageRight);
      }
   }
}
