using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestWithMouse : MonoBehaviour
{
    public InputActionReference horizontalLook;
    public InputActionReference verticalLook;
    public float lookspeed = 1f ;
    public Transform cameraTransform;
    float pitch ;
    float yaw ;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        horizontalLook.action.performed += HandleHorizontalLookChange;
        verticalLook.action.performed += HandleVerticalLookChange;
    }

    void HandleHorizontalLookChange(InputAction.CallbackContext obj)
    {
        yaw += obj.ReadValue<float>();
        transform.localRotation = Quaternion.AngleAxis(yaw*lookspeed, Vector3.up);
        //Debug.Log("1");
    }
    void HandleVerticalLookChange(InputAction.CallbackContext obj)
    {
        pitch -= obj.ReadValue<float>();
        cameraTransform.localRotation = Quaternion.AngleAxis(pitch*lookspeed, Vector3.right);
        //Debug.Log("2");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
