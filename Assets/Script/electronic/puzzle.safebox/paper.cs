using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using TMPro;
public class paper : MonoBehaviour
{
    safeBoxDoor safeBoxDoor;
    TextMeshPro textMeshPro;
    Query query;
    private void Start() {
        query = this.gameObject.AddComponent<Query>();
        textMeshPro = GetComponent<TextMeshPro>();
        safeBoxDoor = query.queryByName(this.transform.root.gameObject,new Regex(@"\bsafeBox.door.base")).First().GetComponent<safeBoxDoor>();
        Debug.Log(textMeshPro == null);
        textMeshPro.text = "<style=\"Title\">"+safeBoxDoor.safeboxPassword.get()+"</style>";
    }
}
