using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareDoor : teleportDoor
{
    public string[] firstSightHint;
    private bool isHinted = false;
    private bool canDetect = false;

    private void OnBecameVisible()
    {
        canDetect = true;
    }

    private void OnBecameInvisible()
    {
        canDetect = false;
    }

    private void Update()
    {
        if (isHinted || !canDetect)
            return;
        var dis = Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);
        if (dis < 100)
        {
            PlotMgr.Instance.ShowPlot(firstSightHint);
            isHinted = true;
        }
    }
}
