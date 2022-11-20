using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 nextMovement;
    Quaternion rotation = Quaternion.identity;
    Quaternion desiredforward;
    private Rigidbody rb;
    private BoxCollider hitbox;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float turnSpeed = 2f;
    private InputManager inputManager;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hitbox = GetComponent<BoxCollider>();
        inputManager = InputManager.Instance;
        float distGround = hitbox.bounds.min.y;
    }
    bool isGround(){
        return Physics.Raycast(transform.position,-Vector3.up,1f);
    }
    void rotate(Vector3 expectForward){
        desiredforward = transform.rotation * Quaternion.AngleAxis(90*expectForward.x,Vector3.up);
        rotation = Quaternion.Slerp(transform.rotation,desiredforward, Time.deltaTime);
        rb.MoveRotation(rotation);
    }
    void move(Vector3 expectForward){
        if(expectForward.y > 0 && isGround()){
            Physics.gravity = Vector3.zero;
        }
        else if(expectForward.y < 0 && isGround()){
            Physics.gravity = Vector3.down*9.8f;
        }

        if(rb.velocity.magnitude > 5*speed){
            rb.velocity = (transform.forward*expectForward.z+transform.up*expectForward.y)*5*speed;
        }else{
            rb.velocity += (transform.forward*expectForward.z+transform.up*expectForward.y)*speed;
        }
        
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        nextMovement = inputManager.movement();
        nextMovement.Normalize();
        move(nextMovement);
        rotate(nextMovement);
        //Debug.Log(nextMovement);
    }
    
    
}
