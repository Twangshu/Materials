using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportDoor : MonoBehaviour
{
    public string destinationName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag==Tags.Player)
        {
            SceneMgr.Instance.GoSceneTo(destinationName,GameManager.Instance.InitNewScene);
        }
    }
}
