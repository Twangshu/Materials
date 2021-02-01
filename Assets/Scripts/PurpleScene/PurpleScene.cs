using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallColor
{
    Null,
    Red,
    Orange,
    Yellow,
    Green,
    Qing,
    Blue,
    Purple
}

public class PurpleScene : Singleton<PurpleScene>
{
    public BallColor curColor;
    public Vector3[] telePos;
    public Vector3 initPos = new Vector3(92.6f, 4.184f, 86.16f);


    public void ReturnToStart()
    {
        GameManager.Instance.TeleportPlayer(telePos[0]);
        curColor = BallColor.Null;
    }

    public void TeleportToNextPlace()
    {
        var index = (int)curColor;
        GameManager.Instance.TeleportPlayer(telePos[index]);
        curColor = (BallColor)(index + 1);
    }
}
