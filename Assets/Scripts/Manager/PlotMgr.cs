using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlotMgr : Singleton<PlotMgr>
{
    public readonly string[] firstPlotContents = { "过着日复一日的生活", "我的世界", "渐渐失去了色彩", "不要", "我不想要这样的生活。" };
    public readonly string[] whenMeetTeleprotContents;
    private CommonTimer firstPlotTime;

    public void ShowPlot(string[] plotContents,Action callback=null)
    {
        EventCenter.Broadcast(EventDefine.ShowPlot, plotContents,callback);
    }


    public override void Awake()
    {
        base.Awake();
        firstPlotTime = new CommonTimer(0, 5, () =>
        {
            ShowPlot(firstPlotContents);
            firstPlotTime = null;
        });
    }

    public override void Update()
    {
        base.Update();
        firstPlotTime?.Update();
    }
}
