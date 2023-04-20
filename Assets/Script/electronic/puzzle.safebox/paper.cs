using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using TMPro;
using System;
using UnityEngine.XR.Interaction.Toolkit;
public class paper : MonoBehaviour
{
    public safeBoxDoor safeBoxDoor;
    TextMeshPro textMeshPro;
    Query query;
    private void Start() {
        query = this.gameObject.AddComponent<Query>();
        textMeshPro = this.transform.Find("Canvas/password").GetComponent<TextMeshPro>();
  
        textMeshPro.text = "<style=\"Title\"><color=\"black\">"+Convert.ToInt32(safeBoxDoor.safeboxPassword.get(),2)+"</color></style>";
        this.gameObject.GetComponent<XRGrabInteractable>().interactionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
    }
    public void reset(){
        textMeshPro.text = "<style=\"Title\">"+Convert.ToInt32(safeBoxDoor.safeboxPassword.get(),2)+"</style>";
    }
}
