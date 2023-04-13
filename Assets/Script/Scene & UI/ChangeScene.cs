using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ChangeScene : MonoBehaviour
{
    public FadeScreen fadeScreen;
    
    public void GoToScene(int sceneindex)
    {
        StartCoroutine(GotoSceneRoutine(sceneindex));
    }

    IEnumerator GotoSceneRoutine(int sceneindex)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);
        SceneManager.LoadScene(sceneindex);
    }
/*
    public void GoToSceneAsync(int sceneindex)
    {
        StartCoroutine(GotoSceneAsyncRoutine(sceneindex));
    }

    IEnumerator GotoSceneAsyncRoutineA(int sceneindex)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);
        AsynceOperation operation = SceneManager.LoadSceneAsync(sceneindex);
        operation.allowSceneActivition = false;

        float timer = 0
        while(timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivition = true;
    }
*/
    public void Start_Scene() 
    {  
        SceneManager.LoadScene("Start_Scene");  
    }  
    public void Tutorial_Scene() 
    {  
        SceneManager.LoadScene("Tutorial_Scene");  
    }  
    public void Building() {

        SceneManager.LoadScene("84Building");  
    }  
}
