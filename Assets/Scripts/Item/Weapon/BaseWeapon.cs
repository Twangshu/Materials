using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    private Animator animitor;


    private void Awake()
    {
        animitor = FindObjectOfType<PlayerAnimitor>().GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
