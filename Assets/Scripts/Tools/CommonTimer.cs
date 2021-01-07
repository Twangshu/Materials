using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

/// <summary>
/// 公用计时器类
/// cloudtian 11.19
/// </summary>
public class CommonTimer
{
    private float maxTime;
    private float curTime;
    private float resetSP;//重置时计时器起点
    private Action callback;

    public CommonTimer(float startTime, float maxTime,Action callback,float resetSP = 0)
    {
        curTime = startTime;
        this.maxTime = maxTime;
        this.callback = callback;
        this.resetSP = resetSP;
    }

    /// <summary>
    /// 到点自动触发并重置计时器
    /// </summary>
    public void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= maxTime)
        {
            callback();
            curTime = resetSP;
        }
    }

    /// <summary>
    /// 到点不触发，需要手动重置，CD类型
    /// </summary>
    public void UpdateCD()
    {
        curTime += Time.deltaTime;
        if (curTime >= maxTime)
        {
            callback();
        }
    }

    public bool isReachTimer()
    {
        if(curTime>=maxTime)
        {
            return true;
        }
        return false;
    }

    public void Reset(float resetSP = 0)
    {
        curTime = resetSP;
    }
}
