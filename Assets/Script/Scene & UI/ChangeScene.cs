using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using Realms;

public class ChangeScene : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public Realm realm;
    public void GoToSceneAsync(int sceneindex)
    {
        StartCoroutine(GotoSceneAsyncRoutine(sceneindex));
    }

    public void newGame()
    {
        realm = Realm.GetInstance();
        realm.Write(()=>{realm.RemoveAll();});
        StartCoroutine(GotoSceneAsyncRoutine(2));
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
