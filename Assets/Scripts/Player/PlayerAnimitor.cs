using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayerMove))]
public class PlayerAnimitor : MonoBehaviour
{
    private Animator animator;
    private int movelayer = 0;
    private readonly string WaitStateName = "WAIT00";
    public bool canDoubleAttack = false;
    public float waitTime = 0f;
    [HideInInspector]
    public PlayerMove playerMove;
    [HideInInspector]
    public GameObject mainCamera;
    public float jumpRatio = 1f;

    private void Awake()
    {
        mainCamera = Camera.main.gameObject;
        playerMove = GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();
        ChangeIdleState();
    }

    void Update()
    {
        GetAnimInput();
        
    }

    private void GetAnimInput()
    {
        //跳跃部分
        if (playerMove.canJump())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioManager.Instance.PlayEffect("Music/SE/jump");
                animator.Play("Jump");
                playerMove.state = PlayerState.Jumping;
                animator.SetBool("isJumping", true);
            }
        }

    }
    private void ChangeIdleState()//定时切换idle状态
    {
        //if (animator.GetCurrentAnimatorClipInfo(movelayer)[0].clip.name.CompareTo(WaitStateName) != 0)//不在休闲状态则不切换到休闲动画
        //{
        //    return;
        //}
        //else
        //{
        //    waitTime += Time.deltaTime;
        //}
        //if(waitTime>5f)
        //{
        //    waitTime = 0;
        //    int index = Random.Range(0, 5);
        //    switch (index)
        //    {
        //        case 1:
        //            Play("wait1");
        //            break;
        //        case 2:
        //            Play("wait2");
        //            break;
        //        case 3:
        //            Play("wait3");
        //            break;
        //        case 4:
        //            Play("wait4");
        //            break;
        //    }       
        //}
    }

    public void StopJumping()
    {
        playerMove.state = PlayerState.Idle;
        animator.SetBool("isJumping", false);
    }

    

    #region 对外接口
    public void SetBool(string name,bool value = true)
    {
        animator.SetBool(name, value);
    }

    public void SetFloat(string name, float value)
    {
        animator.SetFloat(name, value);
    }

    public void SetTrigger(string name)
    {
        animator.SetTrigger(name);
    }

    public void Play(string name,int layer=0,float time=0f)
    {
        animator.Play(name,layer,time);
    }
    #endregion
}
