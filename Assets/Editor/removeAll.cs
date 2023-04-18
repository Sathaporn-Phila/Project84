using UnityEngine;
using UnityEditor;
using Realms;
public class removeAll : MonoBehaviour
{
}

[CustomEditor(typeof(removeAll))][ExecuteInEditMode] // Replace "MyComponent" with the name of your component or object
public class removeAllEditor : Editor
{
    public Realm realm;
    private void OnEnable() {
        realm = Realm.GetInstance();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Remove All"))
        
        {
            realm.Write(()=>{
                realm.RemoveAll();
            });
            Debug.Log("Remove All complete");
        }
    }
}