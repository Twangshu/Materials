using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour {

    private GameObject Player;

    [Header("摄像机敏感度")]
    public float sensitivity = 1;

    private Transform watchPoint; //注视目标点
    [Header("注视目标点高度")]
    public float watchPointHeight = 1.7f; 
    private float AngleLerp; //当前垂直角度 插值系数
    public float AngleMax = 80.0f; //最大垂直角度
    public float AngleMin = -40.0f; //最小垂直角度
    private Vector3 finalVec = new Vector3(); //最终偏移向量
    [Header("摄像机到目标点距离")]
    public float distance = 2.8f; 
    public float distanceMax = 10f; //到目标点最大距离
    public float distanceMin = 5f; //到目标点最小距离
    public float distanceSpeed = 0.3f; //距离增减速度
    public float GroudDis = 1f;
    //public float preventShakeValue = 0.1f;
    //public float cameraSpeed = 10f;
    public Vector3 LookPosOffset = new Vector3(0, 5f, 0);

    private Camera playerCamera = null;
    public Camera PlayerCamera
    {
        get
        {
            if (playerCamera == null)
                playerCamera = GameObject.Find("playerCamera").GetComponent<Camera>();
            return playerCamera;
        }
    }



    void Start()
    {
        Player = GameObject.FindGameObjectWithTag(Tags.Player);
        watchPoint = new GameObject().transform;

    }
    
    private void Update()
    {
        if (!GameManager.Instance.CanOprate)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerCamera.enabled = !playerCamera.enabled;
            if(playerCamera.enabled)
            {
                distance /= 2;
                distanceMax /= 2;
                distanceMin /= 2;
                distanceSpeed /= 2;
            }
            else
            {
                distance *= 2;
                distanceMax *= 2;
                distanceMin *= 2;
                distanceSpeed *= 2;
            }

        }
        ChangeRotationY(); //水平旋转
        ChangeAngle(); //垂直旋转
        ChangeDistance();//调整相机距离
        FinalCameraPos(); //摄像机最终位置
    }

    void ChangeDistance() //滚轮增减 摄像机到目标点距离
    {
        if (Input.GetAxis("Mouse ScrollWheel") == 0)
            return;
        var tempDis = Input.GetAxis("Mouse ScrollWheel") < 0 ? distanceSpeed : -distanceSpeed;
        distance = Mathf.Clamp(distance + tempDis, distanceMin, distanceMax);
    }

    void ChangeRotationY() //水平旋转
    {
        var rotationY = Input.GetAxis("Mouse X") * sensitivity;
        //注视目标点 水平旋转
        watchPoint.Rotate(0, rotationY, 0);
    }

    void ChangeAngle() //垂直旋转
    {
        AngleLerp -= Input.GetAxis("Mouse Y") * sensitivity/50;

        //限制 垂直角度插值系数 最大最小值
        if (AngleLerp > AngleMax / 90.0f) //除90度 获得 最大 垂直角度插值系数
        {
            AngleLerp = AngleMax / 90.0f;
        }
        else if (AngleLerp < AngleMin / 90.0f)
        {
            AngleLerp = AngleMin / 90.0f;
        }

        //判断 当前垂直角度插值系数 正负
        //根据 注视目标点的 后方向量 上方或下方向量 与 当前垂直角度插值系数
        //获得 偏移向量的方向=
            finalVec = AngleLerp > 0 ? Vector3.Lerp(-watchPoint.forward, watchPoint.up, AngleLerp): 
                Vector3.Lerp(-watchPoint.forward, -watchPoint.up, -AngleLerp);

        finalVec.Normalize(); 
        finalVec *= distance; //设定 偏移向量的长度
    }

    void FinalCameraPos() //摄像机最终位置
    {
        //注视目标点位置 跟随 玩家位置
        var PointPos = Player.transform.position + LookPosOffset;
        PointPos.y += watchPointHeight; //修改 注视目标点 高度
        //弹簧移动效果 插值实现
        watchPoint.position = Vector3.Lerp(watchPoint.position, PointPos, 0.9f);

        //摄像机位置 根据注视目标点位置 进行偏移
        Vector3 cameraPos = watchPoint.position + finalVec;
        //弹簧移动效果 插值实现 相机防抖(后面发现把这些插值之类的去掉反而不抖了)
        // if(Vector3.Distance(transform.position,cameraPos) > preventShakeValue)
        //检测碰撞地面
        cameraPos = CheckCollider(cameraPos);
        transform.position = cameraPos;// Vector3.Lerp(transform.position, cameraPos, cameraSpeed * Time.deltaTime);
        transform.LookAt(watchPoint.position);
        //将行动的前方设置成相机的前方而不是人物的前方
        GameManager.Instance.Forward = new Vector3(gameObject.transform.forward.x, 0, gameObject.transform.forward.z).normalized;
    }

    private Vector3 CheckCollider(Vector3 targetPos)
    {
        //Debug.DrawRay(transform.position,Vector3.down*10,Color.red,10f);
        Physics.Raycast(transform.position, Vector3.down,out var hitInfo, 10f, LayerMask.GetMask("Ground"));
        if(hitInfo.collider!=null)
        {
            if (hitInfo.point.y + GroudDis == targetPos.y)
                return targetPos;
            //类原神那种相机快着地时直接平移视角
            return hitInfo.point.y + GroudDis > targetPos.y ? new Vector3(targetPos.x, hitInfo.point.y + GroudDis, targetPos.z) : targetPos;
        }
        return targetPos;
    }


}
