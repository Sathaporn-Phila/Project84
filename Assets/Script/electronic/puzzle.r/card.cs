using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Realms;
public class card : MonoBehaviour
{
    Realm _realm;
    CardData cardData;
    bool isAnimated;
    float speed = 2f;
    Rigidbody rb;
    [SerializeField] float distance = 1;
    bool WanttoQuit(){
        this.saveObjectPosition();
        return true;
    }
    private void saveObjectPosition(){
        _realm.Write(()=>{
            cardData.transformModel.Position = this.transform.position;
            cardData.transformModel.Rotation = this.transform.rotation;
        });
    }
    void Start()
    {
        _realm = Realm.GetInstance();
        string path = this.FindPath(this.transform);
        cardData = _realm.Find<CardData>(path);
        if(cardData is null){
            _realm.Write(()=>{
                cardData = _realm.Add(new CardData(path,this.transform));
                cardData.isAnimated = false;
            });
        }else{
            this.transform.position = cardData.transformModel.Position;
            this.transform.rotation = cardData.transformModel.Rotation;
        }
        isAnimated = cardData.isAnimated;
        rb = GetComponent<Rigidbody>();

    }
    public void Animated(){
        
        if(!isAnimated){
            float step = Time.deltaTime;
            StartCoroutine("SmoothLerp",0.5);     
            isAnimated = true;
            _realm.Write(()=>{
                cardData.isAnimated = true;
            });
            this.gameObject.GetComponent<XRGrabInteractable>().interactionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        }
        
    }
    private IEnumerator SmoothLerp (float time){
 
        Vector3 startingPos  = transform.position;
        Vector3 finalPos = transform.position + distance*transform.TransformDirection(Vector3.forward);
        Debug.Log(Vector3.Distance(finalPos,startingPos));
        Vector3 dir = transform.TransformDirection(Vector3.forward);
        float elapsedTime = 0;
         
        while (elapsedTime < time)
        {
            rb.MovePosition(Vector3.Lerp(startingPos,finalPos,elapsedTime/time));
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    string FindPath(Transform t){
        string path = t.name;

        while (t.parent != null) {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
    
}
