using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneMgr : Singleton<SceneMgr>
{

    public string loadingSceneName;
    public Action loadingCompleteCallback;
    public void GoSceneTo(string SceneName, Action callback = null)
    {
        loadingSceneName = SceneName;
        GoToWaitingScene();
        //if(processSlider == null)
        //{
        //    processSlider = GameObject.Find("processSlider").GetComponent<Slider>();
        //}
        if (callback != null)
            loadingCompleteCallback = callback;
    }

    
    public void GoToWaitingScene()
    {

        SceneManager.LoadScene("waiting");
        Log.LogDebug("waiting场景加载完成");
    }
}
