using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChange : MonoBehaviour
{
    public GameObject Option_UI;
    public GameObject Main_Menu_UI;

    public void open_option()
    {
        Main_Menu_UI.SetActive(false);
        Option_UI.SetActive(true);
    }
    public void close_option()
    {
        Main_Menu_UI.SetActive(false);
        Option_UI.SetActive(true);
        
    }
}
