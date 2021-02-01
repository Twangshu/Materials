using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pur_TeleportBall : MonoBehaviour
{
    public BallColor m_color;
    public BallColor shouldColor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals(Tags.Player))
        {
            if(PurpleScene.Instance.curColor.Equals(shouldColor))
            {
                PurpleScene.Instance.TeleportToNextPlace();
            }
            PurpleScene.Instance.ReturnToStart();
        }
    }
}
