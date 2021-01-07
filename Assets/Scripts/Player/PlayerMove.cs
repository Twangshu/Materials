using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.XR;
using UnityEngine.PlayerLoop;
using Boo.Lang.Runtime;



public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Jumping,
    Attacking,
    beAttacking,
    Flying,
    Skill,
    Dancing,
    Dialog,
}

public class PlayerMove : MonoBehaviour
{
    private PlayerAnimitor animator;
    private CommonTimer ChangeIdleTimer;
    private CharacterController CharacterController;

    [SerializeField]
    public PlayerState state = PlayerState.Idle;

    /// <summary>
    /// 动画相关
    /// </summary>
   // public float speedMax = 4f;
    [Header("移动速度")]
    public float speed = 4f;
    [Header("转向速度")]
    public float turnSpeed = 1f;
    public Vector3 downSpeed = new Vector3(0, -0.1f, 0);//下落速度
    private bool isRunState = true;
    private bool isDanceState = false;
    public float h;
    public float v;
    private Tween tween;//移动动画

    public float timescale = 1;

    /// <summary>
    /// 闪避冲刺部分
    /// </summary>
    public float rushTimer;
    public float rushTime = 0.1f;
    public bool canRush = false;

    void Start()
    {
        ChangeIdleTimer = new CommonTimer(0,5f, ChangeIdleState,-10f);
        animator = transform.GetComponent<PlayerAnimitor>();
        CharacterController = GetComponent<CharacterController>();
        EventCenter.AddListener(EventDefine.Teleport, (Vector3 pos) => Teleport(pos));
    }


    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.Teleport, (Vector3 pos) => Teleport(pos));
    }
    private void FixedUpdate()
    {
        CheckRush();
        GetInput();
    }

    private void CheckRush()
    {
        if (canRush)
        {
            rushTimer += Time.deltaTime;
            if (rushTimer > rushTime)
            {
                canRush = false;
                rushTimer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (canRush)
            {
                //rush
                canRush = false;
            }
            else
            {
                canRush = true;
            }
        }
    }

    private void GetInput()
    {
        if (isDanceState || !GameManager.Instance.CanOprate)
            return;
        tween.Kill();//终止动画
        GetAnimInput();
    }
    private void Update()
    {
        if (!GameManager.Instance.CanOprate)
            return;
        if (Input.GetKeyDown(KeyCode.R))
            isRunState = !isRunState;
        UpdateTimer();
    }



    private void UpdateTimer()
    {
        if (state.Equals(PlayerState.Idle))
            ChangeIdleTimer.Update();
        else
            ChangeIdleTimer.Reset();
    }
    private void ChangeIdleState()
    {
        var index = Random.Range(1, 5);
        var animName = "wait" + index.ToString();
        animator.Play(animName);
    }

    private void Teleport(Vector3 targetPos)
    {
        GameManager.Instance.CanOprate = false;
        transform.position = targetPos;
        StartCoroutine(IETeleport());
    }
    IEnumerator IETeleport()
    {
        yield return new WaitForSeconds(0.01f);
        GameManager.Instance.CanOprate = true;
    }

    private void GetAnimInput()
    {
        CharacterController.Move(downSpeed * transform.localScale.x);
        if (canMove())
        {
            v = Input.GetAxis("Vertical") * (isRunState ? 2 : 1);
            h = Input.GetAxis("Horizontal") * (isRunState ? 2 : 1);
            if (!state.Equals(PlayerState.Jumping))
                animator.SetFloat("Forward", Mathf.Max(Mathf.Abs(h), Mathf.Abs(v)));
            Move();
        }
        else
        {
            AudioManager.Instance.StopEffect("Music/SE/walk");
        }
        

    }
    private void Move()
    {
        Vector3 normal = Vector3.Cross(GameManager.Instance.Forward, Vector3.up);
        float movespeed = isRunState ? speed : speed / 1.5f;

        if (h != 0 || v != 0)
            CharacterController.SimpleMove(v * GameManager.Instance.Forward * movespeed + h * (-normal) * movespeed);
        if (h != 0 && v != 0)
        {
            float turnAng = h > 0 ? 90 : 270;
            float moveAng = v > 0 ? 0 : 180;
            float finalAng = h < 0 && v > 0 ? 315 : (turnAng + moveAng) / 2;
            //tween1 = DOTween.To(() => transform.eulerAngles, x => transform.eulerAngles = x, new Vector3(transform.eulerAngles.x, (Camera.main.transform.eulerAngles.y + finalAng) % 360, transform.eulerAngles.z), Time.fixedDeltaTime * turnSpeed);//如果超出360=会往复弹跳
            tween = transform.DORotate(new Vector3(transform.eulerAngles.x, (Camera.main.transform.eulerAngles.y + finalAng) % 360), Time.fixedDeltaTime * turnSpeed);
        }
        else
        {
            if (h != 0)
            {
                float turnAng = h > 0 ? 90 : 270;
                tween = transform.DORotate(new Vector3(transform.eulerAngles.x, (Camera.main.transform.eulerAngles.y + turnAng) % 360), Time.fixedDeltaTime * turnSpeed);

            }
            if (v != 0)
            {
                float moveAng = v > 0 ? 0 : 180;
                tween = transform.DORotate(new Vector3(transform.eulerAngles.x, (Camera.main.transform.eulerAngles.y + moveAng) % 360), Time.fixedDeltaTime * turnSpeed);
            }
        }
        if (h != 0 || v != 0)
        {
            if (!state.Equals(PlayerState.Jumping))
                state = isRunState ? PlayerState.Running : PlayerState.Walking;

            animator.SetBool("isMoving", true);
            if(state.Equals(PlayerState.Jumping))
            {
                AudioManager.Instance.StopEffect("Music/SE/walk");
            }
            else
            {
                var playSpeed = isRunState ? 1.5f : 1f;
                AudioManager.Instance.PlayEffect("Music/SE/walk", playSpeed);
            }
        }
        else
        {
            if (state.Equals(PlayerState.Running)||state.Equals(PlayerState.Walking))
                state = PlayerState.Idle;
            animator.SetBool("isMoving", false);
            AudioManager.Instance.StopEffect("Music/SE/walk");
        }
    }

    public bool canMove()
    {
        return state != PlayerState.Attacking && state != PlayerState.beAttacking && state != PlayerState.Flying &&
            state!=PlayerState.Skill;
    }

    public bool canJump()
    {
        return state.Equals(PlayerState.Idle) || state.Equals(PlayerState.Walking) || state.Equals(PlayerState.Running);
    }

    public bool canAttack()
    {
        return GameManager.Instance.CanOprate&&!state.Equals(PlayerState.Jumping) && !state.Equals(PlayerState.beAttacking) &&!state.Equals(PlayerState.Skill);
    }

    public bool canDance()
    {
        return state.Equals(PlayerState.Idle);
    }

    public bool canDialog()
    {
        return state.Equals(PlayerState.Idle) || state.Equals(PlayerState.Running) || state.Equals(PlayerState.Walking);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals(Tags.Npc))
        {
            //显示按F的UI
            if(Input.GetKeyDown(KeyCode.F)&&GameManager.Instance.CanOprate&&canDialog())
            {
                animator.SetBool("isMoving", false);
                animator.Play("wait00");
                other.SendMessage("ShowDialogs");
            }
        }
        else if(other.tag.Equals(Tags.PickItem))
        {
            if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.CanOprate)
            {
                other.SendMessage("BePicked");
            }
        }
    }
}
