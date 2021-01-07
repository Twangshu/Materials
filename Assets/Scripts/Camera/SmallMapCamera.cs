using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMapCamera : MonoBehaviour
{
    private float distance =40f;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.Player);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + Vector3.up * distance ;
    }
}
