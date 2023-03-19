using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
public class player : MonoBehaviour
{
    bool spawnFromSave;
    string playerPath = Directory.GetCurrentDirectory()+"/Assets/Script/player/player.json";
    public class PlayerData {
        public Vector3 position;
        public Quaternion rotation;
        public string checkpoint;
        public PlayerData(Vector3 pos,Quaternion rot){
            position = pos;
            rotation = rot;
        } 
    }
    void Start()
    {
        if(File.Exists(playerPath) && spawnFromSave){
            var jsonData = File.ReadAllText(playerPath);
            Vector3 position = JsonUtility.FromJson<PlayerData>(jsonData).position;
            Quaternion rotation = JsonUtility.FromJson<PlayerData>(jsonData).rotation;
            this.transform.position = position;
            this.transform.rotation = rotation;
        }
    }

    private void OnApplicationQuit() { //เก็บข้อมูล player เมื่อออก
        savePlayerPosition();

    }
    private void savePlayerPosition(){
        PlayerData data = new PlayerData(this.transform.position,this.transform.rotation);
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(playerPath,jsonData);
        

    }
    public void SetCheckpoint(string name){
        savePlayerPosition();
        var jsonData = File.ReadAllText(playerPath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
        data.checkpoint = name;
        jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(playerPath,jsonData);
    }
}
