using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using Cloudtian;



public class ResManager : Singleton<ResManager>
{
    public Dictionary<string, ResItem> m_cache = new Dictionary<string, ResItem>();
    public Queue<ResItem> m_waitLoadQueue = new Queue<ResItem>();
    public List<ResItem> m_loadingItems = new List<ResItem>();
    public int maxLoadingCount = 5;

    public override void Update()
    {
        base.Update();
        while (maxLoadingCount > m_loadingItems.Count && m_waitLoadQueue.Count > 0)
        {
            var item = m_waitLoadQueue.Dequeue(); //从wait列表中移除
            item.LoadAsync();
            m_loadingItems.Add(item);
        }

        var count = m_loadingItems.Count;
        for (int i = 0; i < m_loadingItems.Count; i++)
        {
            if (m_loadingItems[i].async.isDone)
            { 
                var item = m_loadingItems[i];
                m_loadingItems.Remove(item);
                item.ExcuteCallBack();
            }
        }
    }

    public ResItem LoadAsync(string filePath, Action<object> callBack) 
    {
        m_cache.TryGetValue(filePath,out var item);
        if (null != item) //判断资源是否加载过
        {
            item.refCount++;
            return item;
        }
        else
        {
            item = InitItem(filePath, callBack);
            if (item != null)
            {
                m_cache.Add(filePath, item);
                item.AddCallBack(callBack);
                m_waitLoadQueue.Enqueue(item);
                return item;
            }
        }
        return null;
    }

    private ResItem InitItem( string filePath, Action<object> callback)
    {
        ResItem item = null;
        item = new ResItem(filePath, callback);
        return item;
    }


}