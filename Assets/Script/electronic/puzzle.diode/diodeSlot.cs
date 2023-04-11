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
    wireQuery wireQueryGroup;
    void Awake()
    {
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
        wireDiodeSlot = this.transform.parent.GetComponent<wireDiodeSlot>();
    }

    void Start(){
        //StartCoroutine(spawnTest());
    }
    IEnumerator spawnTest(){
        
        yield return new WaitForSeconds(Random.Range(0,2));
        diode = (GameObject)Resources.Load("Prefabs/electronic/diode");
        Instantiate(diode,transform.position+Vector3.up,transform.rotation);
        
    }
    
    void OnTriggerEnter(Collider other){
        if(Regex.IsMatch(other.gameObject.name,@"\bdiode")){
            CapsuleCollider collider = other.GetComponent<CapsuleCollider>();
            collideGameObject = collider.gameObject;
            Ray cathodeDirection = new Ray(transform.position+Vector3.down*0.1f,other.transform.TransformDirection(-Vector3.forward));
            float angle = Mathf.Atan2(cathodeDirection.direction.z,cathodeDirection.direction.x)*Mathf.Rad2Deg - Mathf.Atan2(wireDiodeSlot.toggleRay.getRay().direction.z,wireDiodeSlot.toggleRay.getRay().direction.x)*Mathf.Rad2Deg;

            //change current direction when diode change direction
            if(Mathf.Abs(angle) < 90 ){
                wireDiodeSlot.toggleRay.setDirection(-cathodeDirection.direction);
                //Debug.Log(wireDiodeSlot.scale);
            }

        }     
    }
    void OnTriggerExit(Collider other) {
        if(Regex.IsMatch(other.gameObject.name,@"\bdiode")){
            collideGameObject = null;
        }
    }
}
