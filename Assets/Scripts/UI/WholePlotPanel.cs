using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WholePlotPanel : BaseUIPanel
{
    public WholePlotPanel()
    {
        ShowType = EventDefine.ShowWholePlotPanel;
        HideType = EventDefine.HideWholePlotPanel;
    }
    public enum textState
    {
        Showing,
        WaitToContinue,
        Finish,
        NULL,
        WinHint,
    }
    public string []showContents;
    private string m_showingContent = "";
    public Text contentText;
    public Image backGroundImage;
    public Image m_continueImage;
    public float showOnceTime = 1.5f;
    public float showOnceTimer = 0f;
    public bool canContinue = false;
    public int showIndex = 0;
    public Action showComplete;
    private textState m_state = textState.NULL;

    public override void Awake()
    {
        backGroundImage = GetComponent<Image>();
        contentText = GetComponentInChildren<Text>();
        EventCenter.AddListener<string[], Action>(EventDefine.ShowPlot, ShowSlotContents);
        base.Awake();
    }

    protected override void OnDestroy()
    {
        EventCenter.RemoveListener<string[], Action>(EventDefine.ShowPlot, ShowSlotContents);
        base.OnDestroy();
    }

    public void ShowSlotContents(string[] contents,Action callback=null)
    {
        EventCenter.Broadcast(ShowType);
        showIndex = 0;
        showContents = contents;
        showComplete = callback;
        ShowSlotContent();
    }

    public void ShowSlotContent()
    {
        m_showingContent = showContents[showIndex++];
        m_continueImage?.gameObject.SetActive(false);
        m_state = textState.Showing;
    }

    public override void Update()
    {
        base.Update();
        if(m_state.Equals(textState.Showing))
        {
            showOnceTime = m_showingContent.Length * 0.2f;
            showOnceTimer += Time.deltaTime;
            contentText.text = m_showingContent.Substring(0, Mathf.Clamp((int)Mathf.Floor(m_showingContent.Length * (showOnceTimer / showOnceTime)),0,m_showingContent.Length));
            if(showOnceTimer>showOnceTime)
            {
                m_state = showIndex == showContents.Length ? textState.Finish : textState.WaitToContinue;
                showOnceTimer = 0;
                m_continueImage?.gameObject.SetActive(true);
            }

            if(Input.GetMouseButton(0))
            {
                showOnceTimer = showOnceTime;
            }
        }

        if(Input.GetMouseButton(0) && m_state.Equals(textState.WaitToContinue))
        {
            ShowSlotContent();
        }
        else if(Input.GetMouseButton(0) && m_state.Equals(textState.Finish))
        {
            if (showComplete != null)
                showComplete();
            HideThis();
        }
        
        
    }

    public override void ShowThis()
    {
        base.ShowThis();
        GameManager.Instance.CanOprate = false;
    }

    public override void HideThis()
    {
        base.HideThis();
        GameManager.Instance.CanOprate = true;
    }
}
