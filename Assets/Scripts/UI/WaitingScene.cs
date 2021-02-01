using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingScene : MonoBehaviour
{
    private Slider processSlider;
    private CommonTimer loadTimer;
    private bool isTimeOut = false;
    private int curProgressValue = 0;
    private AsyncOperation async;


    private void Awake()
    {
        processSlider = GameObject.Find("processSlider").GetComponent<Slider>();
        isTimeOut = false;
        loadTimer = new CommonTimer(0, 5, () =>
        {
            isTimeOut = true;
            Log.LogError("加载超时");
        });
        StartCoroutine(LoadSceneAsync(SceneMgr.Instance.loadingSceneName));
    }

    IEnumerator LoadSceneAsync(string SceneName)
    {
        yield return new WaitForSeconds(0.001f);
        async = SceneManager.LoadSceneAsync(SceneName);
        async.allowSceneActivation = false;
        yield return async;
        //while (!async.isDone && !isTimeOut)
        //{
        //    loadTimer.Update();
        //    Log.LogDebug(async.progress);
            
        //    //if (async.progress < 0.9f)
        //    //    processSlider.value = async.progress;
        //    //else
        //    //    processSlider.value = 1.0f;


        //    //if (processSlider.value >= 0.9)
        //    //{
        //    //    async.allowSceneActivation = true;
        //    //}

        //    yield return null;
        //}
    }

    void Update()
    {

        int progressValue = 100;

        if (curProgressValue < progressValue)
        {
            curProgressValue++;
        }


        processSlider.value = curProgressValue / 100f;

        if (curProgressValue == 100)
        {
            if (SceneMgr.Instance.loadingCompleteCallback != null)
                SceneMgr.Instance.loadingCompleteCallback();
            async.allowSceneActivation = true;//启用自动加载场景  
        }
    }
}
