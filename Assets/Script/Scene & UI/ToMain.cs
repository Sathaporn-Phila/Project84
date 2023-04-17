using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using Realms;

public class ToMain : MonoBehaviour
{
    public FadeScreen fadeScreen;
   
    public void GoToSceneAsync(int sceneindex)
    {
        StartCoroutine(GotoSceneAsyncRoutine(sceneindex));
    }

    IEnumerator GotoSceneAsyncRoutine(int sceneindex)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneindex);
        asyncOperation.allowSceneActivation = false;

        float timer = 0;
        while(timer <= fadeScreen.fadeDuration && !asyncOperation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }
}
