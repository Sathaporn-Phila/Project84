using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class diodeSlot : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject diode;
    public GameObject collideGameObject;
    wireDiodeSlot wireDiodeSlot;
    void Start()
    {
        diode = (GameObject)Resources.Load("Prefabs/electronic/diode");
        Instantiate(diode,this.gameObject.transform.position+Vector3.up*3,this.gameObject.transform.rotation);
        wireDiodeSlot = this.transform.parent.GetComponent<wireDiodeSlot>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter(Collider other){
        if(Regex.IsMatch(other.gameObject.name,@"\bdiode")){
            CapsuleCollider collider = other.GetComponent<CapsuleCollider>();
            collideGameObject = collider.gameObject;
            Ray cathodeDirection = new Ray(other.transform.position-(Vector3.down*collider.radius/2),collider.transform.TransformDirection(-Vector3.forward));

            //change current direction when diode change direction
            if(cathodeDirection.direction != wireDiodeSlot.toggleRay.getRay().direction){
                wireDiodeSlot.toggleRay.setDirection(-cathodeDirection.direction);
                GameObject wireYHit = wireDiodeSlot.findParentObjectHit(wireDiodeSlot.toggleRay.getRay(),wireDiodeSlot.scale,7);
                wireYHit.GetComponent<wireY>().controlRay.Switchflow(wireDiodeSlot.gameObject);
            }

        }     
    }
}
