using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int id = 1;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            PlotMgr.Instance.ShowPlot(PlotMgr.Instance.firstPlotContents);
        }
    }
}
