using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChange : MonoBehaviour
{
    public GameObject Option_UI;
    public GameObject Main_Menu_UI;

    public void open_option()
    {
        
        Option_UI.SetActive(true);
    }
    public void close_option()
    {
        Option_UI.SetActive(false);
        
    }
}
