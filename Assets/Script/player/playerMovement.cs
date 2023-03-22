using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using Realms;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Realm _realm;
    private PlayerData playerInfo;
    Vector3 nextMovement;
    Quaternion rotation,desiredforward = Quaternion.identity;
    public bool spawnFromSave;
    private Rigidbody rb;
    private CapsuleCollider hitbox;
    [SerializeField]
    private float speed = 2f,turnSpeed = 2f;
    [SerializeField]
    private InputManager inputManager;
    bool WanttoQuit(){
        this.savePlayerPosition();
        return true;
    }
    void Start()
    {
        Application.wantsToQuit += WanttoQuit;
        _realm = Realm.GetInstance();
        Debug.Log($"Realm is located at: {_realm.Config.DatabasePath}");
        playerInfo = _realm.Find<PlayerData>("player");

    
        rb = GetComponent<Rigidbody>();
        hitbox = GetComponent<CapsuleCollider>();
        inputManager = InputManager.Instance;
        float distGround = hitbox.bounds.min.y;

        if(playerInfo is null){
            _realm.Write(()=>{
                playerInfo = _realm.Add(new PlayerData("player", this.transform));
            });
        }else{
            rb.MovePosition(playerInfo.transformModel.Position);
            rb.MoveRotation(playerInfo.transformModel.Rotation);
        }
        
            /*var jsonData = File.ReadAllText(playerPath);
            Dictionary<string,PlayerData> playerData = JsonConvert.DeserializeObject<Dictionary<string,PlayerData>>(jsonData);
            //Debug.Log(playerData["player"].position);
            Vector3 position = playerData["player"].position;
            Quaternion rotation = playerData["player"].rotation;
            rb.MovePosition(position);
            rb.MoveRotation(rotation);*/
        

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
    
    private void savePlayerPosition(){
        _realm.Write(()=>{
            playerInfo.transformModel.Position = this.transform.position;
            playerInfo.transformModel.Rotation = this.transform.rotation;
            playerInfo.transformModel.Scale = this.transform.localScale;
        });
        
        /*PlayerData data = new PlayerData(rb.position,rb.rotation);
        string jsonData = JsonConvert.SerializeObject(new Dictionary<string,PlayerData>(){{"player",data}},new JsonSerializerSettings()
                        { 
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
        if(File.Exists(playerPath)){
            
        }else{
            File.WriteAllText(playerPath,jsonData);
        }*/
        //string jsonData = JsonUtility.ToJson(data);
        
        
        
        /*PlayerPrefs.SetFloat("Player Position X",rb.position.x);
        PlayerPrefs.SetFloat("Player Position Y",rb.position.y);
        PlayerPrefs.SetFloat("Player Position Z",rb.position.z);
        PlayerPrefs.Save();*/
    }
}
