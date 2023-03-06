using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class card : MonoBehaviour
{
    bool isAnimated;
    float speed = 0.1f;
    [SerializeField] float distance = 4;
    void Start()
    {
        isAnimated = false;
    }
    public void Animated(){
        if(!isAnimated){
            float step = speed*Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position,transform.position+distance*transform.TransformDirection(Vector3.forward),step);
            isAnimated = true;
            this.gameObject.GetComponent<XRGrabInteractable>().interactionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        }
        
    }
}
