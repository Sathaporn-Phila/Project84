using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 nextMovement;
    Quaternion rotation,desiredforward = Quaternion.identity;
    private Rigidbody rb;
    private BoxCollider hitbox;
    [SerializeField]
    private float speed = 2f,turnSpeed = 2f;
    [SerializeField]
    private InputManager inputManager;
    //[Serializable]
    public class PlayerData {
        public Vector3 position;
        public PlayerData(Vector3 pos){
            position = pos;
        } 
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hitbox = GetComponent<BoxCollider>();
        inputManager = InputManager.Instance;
        float distGround = hitbox.bounds.min.y;
        

        /*if(PlayerPrefs.HasKey("Player Position X")){
            Vector3 position = new Vector3(
                                            PlayerPrefs.GetFloat("Player Position X"),
                                            PlayerPrefs.GetFloat("Player Position Y"),
                                            PlayerPrefs.GetFloat("Player Position Z"));
            rb.MovePosition(position);
        }*/
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
    private void OnApplicationQuit() { //เก็บข้อมูล player เมื่อออก
        savePlayerPosition();

    }
    private void savePlayerPosition(){
        PlayerData data = new PlayerData(rb.position);
        string filename = "player.json";
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log(jsonData);
        File.WriteAllText(Directory.GetCurrentDirectory()+"/Assets/Script/player/"+filename,jsonData);
        //string jsonData = JsonUtility.ToJson(data);
        
        
        
        /*PlayerPrefs.SetFloat("Player Position X",rb.position.x);
        PlayerPrefs.SetFloat("Player Position Y",rb.position.y);
        PlayerPrefs.SetFloat("Player Position Z",rb.position.z);
        PlayerPrefs.Save();*/
    }
}
