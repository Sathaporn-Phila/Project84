using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ChangeScene : MonoBehaviour
{
    public void Start_Scene() {  
        SceneManager.LoadScene("Start_Scene");  
    }  
    public void Tutorial_Scene() {  
        SceneManager.LoadScene("Tutorial_Scene");  
    }  
    public void Building() {  
        SceneManager.LoadScene("84Building");  
    }  
}
