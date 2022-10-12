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
    [SerializeField]
    private float speed = 100f;
    [SerializeField]
    public float turnSpeed = 90f;
    private InputManager inputManager;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        nextMovement = inputManager.movement();
        nextMovement.Normalize();
        desiredforward = transform.rotation * Quaternion.AngleAxis(90*nextMovement.x,Vector3.up);
        rotation = Quaternion.Slerp(transform.rotation,desiredforward, turnSpeed * Time.deltaTime);
        rb.MoveRotation(rotation);
        rb.velocity += nextMovement;
        //rb.velocity = new Vector3
    }
    
    
}
