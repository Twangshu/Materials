using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cloudtian;

public class ResItem
{
    public object m_asset;
    public string filePath;
    public int refCount;
    public ResourceRequest async;
    public Action<object> callback;


    public ResItem(string filePath, Action<object> callback = null) 
    {
        m_asset = null;
        this.filePath = filePath;
    }

    public void LoadAsync()
    {
        async = Resources.LoadAsync(filePath);
    }

    public void AddCallBack(Action<object> callback)
    {
        this.callback = callback;
    }

    public void ExcuteCallBack()
    {
        if(async.asset == null)
        {
            Log.LogAssert("加载", filePath, "失败");
            return;
        }
        callback(async.asset);
    }
    
}

