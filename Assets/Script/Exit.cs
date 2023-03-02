using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject Exit_Confirm_Panel;
    public GameObject Main_Menu_UI;
    public void button_exit()
    {
    
        Application.Quit();

    }
    public void enable_confirmation()
    {   
        
        Exit_Confirm_Panel.SetActive(true);
    }
    public void disable_confirmation()
    {
        Exit_Confirm_Panel.SetActive(false);
        
    }
}