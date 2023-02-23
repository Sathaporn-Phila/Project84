using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject confirmation_panel;
    public void button_exit()
    {
    
        Application.Quit();

    }
    public void enable_confirmation()
    {
        confirmation_panel.SetActive(true);
    }
    public void disable_confirmation()
    {
        confirmation_panel.SetActive(false);
    }
}