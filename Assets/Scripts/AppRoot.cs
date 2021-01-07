﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppRoot : MonoBehaviour
{
    public static AppRoot Instance;
    private List<ISingleton> m_managers = new List<ISingleton>();
    /// <summary>
    /// 调试数据
    /// </summary>

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitPreviously();

    }
 
    void Update()
    {
        for (int i = 0; i < m_managers.Count; i++)
        {
            m_managers[i].Update();
        }
    }

    private void FixedUpdate()
    {
    }

    public void InitPreviously()
    {
        AddManager(UIManager.Instance);
        AddManager(ResManager.Instance);
        AddManager(GameManager.Instance);
        AddManager(SceneMgr.Instance);
    }

    public void AddManager(ISingleton mgr)
    {
        m_managers.Add(mgr);
        mgr.Awake();
    }
}
