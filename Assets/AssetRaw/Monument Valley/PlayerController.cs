using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public bool walking = false;
    Sequence s;

    [Space]

    public Transform currentCube;
    public Transform clickedCube;
    public Transform indicator;

    [Space]

    public List<Transform> finalPath = new List<Transform>();

    private float blend;

    void Start()
    {
        RayCastDown();
    }

    void Update()
    {

        //GET CURRENT CUBE (UNDER PLAYER)

        RayCastDown();

        if (currentCube.GetComponent<Walkable>().movingGround)
        {
            transform.parent = currentCube.parent;
        }
        else
        {
            transform.parent = null;
        }

        // CLICK ON CUBE

        if (Input.GetMouseButtonDown(0))
        {
           
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); RaycastHit mouseHit;

            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                if (mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    ///用于解决在行走路程中选择新终点后产生的Bug
                    if (finalPath.Count != 0)
                    {
                        s.Kill();
                        Clear();
                    }

                    clickedCube = mouseHit.transform;
                    DOTween.Kill(gameObject.transform);//立即释放tweener
                    finalPath.Clear();
                    FindPath();

                    blend = transform.position.y - clickedCube.position.y > 0 ? -1 : 1;//用于计算上下行走动画

                    indicator.position = mouseHit.transform.GetComponent<Walkable>().GetWalkPoint();

                    //控制点击特效的粒子系统
                    Sequence s1 = DOTween.Sequence();
                    s1.AppendCallback(() => indicator.GetComponentInChildren<ParticleSystem>().Play());
                    s1.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.white, .1f));
                    s1.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.black, .3f).SetDelay(.2f));
                    s1.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.clear, .3f));

                }
            }
        }
    }

    void FindPath()
    {
        
        List<Transform> nextCubes = new List<Transform>();//所有可能性的路径
        List<Transform> pastCubes = new List<Transform>();
        
        foreach (WalkPath path in currentCube.GetComponent<Walkable>().possiblePaths)//possiblePaths是预设好的
        {
            if (path.active)//是通路
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = currentCube;
            }
        }

        pastCubes.Add(currentCube);

        ExploreCube(nextCubes, pastCubes);
        BuildPath();
    }
    //双向寻找通路，直到一个方向找到点击的终点
    void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)//pastcubes
    {
        Transform current = nextCubes.First();//获取第一个元素
        //可以添加参数，如nextCubes.First(t => t.position == new Vector3(1,1,1));
        nextCubes.Remove(current);

        if (current == clickedCube)//只走一步的情况
        {
            return;
        }

        foreach (WalkPath path in current.GetComponent<Walkable>().possiblePaths)
        {
            if (!visitedCubes.Contains(path.target) && path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = current;
            }
        }

        visitedCubes.Add(current);

        if (nextCubes.Any())//如果还有就继续加
        {
            ExploreCube(nextCubes, visitedCubes);
        }
    }

    void BuildPath()
    {
        Transform cube = clickedCube;
        while (cube != currentCube)
        {
            finalPath.Add(cube);//从终点到起点添点路径方块
            if (cube.GetComponent<Walkable>().previousBlock != null)
                cube = cube.GetComponent<Walkable>().previousBlock;
            else
                return;//没找到之前为当前点，既没有通路直接返回
        }

       // finalPath.Insert(0, clickedCube);
       
        FollowPath();
    }

    void FollowPath()
    {
        s = DOTween.Sequence();//像动画编辑器一样编辑Dotween动画

        walking = true;

        for (int i = finalPath.Count - 1; i >= 0; i--)
        {
            float time = finalPath[i].GetComponent<Walkable>().isStair ? 1.5f : 1;
            //附加一个tweener，由于是每格移动补间，所以在视觉上有匀速的效果
            s.Append(transform.DOMove(finalPath[i].GetComponent<Walkable>().GetWalkPoint(), .2f * time).SetEase(Ease.Linear));//设置方式为缓动
            
            if(!finalPath[i].GetComponent<Walkable>().dontRotate)
            {                //在最后一个tweener的前面连接一个tweener使其和最后一个tweener同时播放
                s.Join(transform.DOLookAt(finalPath[i].position, .1f, AxisConstraint.Y, Vector3.up));//改变玩家朝向,在Y轴上约束
            }

        }

        if (clickedCube.GetComponent<Walkable>().isButton)//如果最后踩到的是按钮地板，则播放按钮对应动画
        {
            s.AppendCallback(()=>GameManager2.instance.RotateRightPivot());//添加回调动画
        }

        s.AppendCallback(() => Clear());//清空路径和标记的前方块，停止动画
    }

    void Clear()
    {
        foreach (Transform t in finalPath)
        {
            t.GetComponent<Walkable>().previousBlock = null;
        }
        finalPath.Clear();
        walking = false;
    }

    public void RayCastDown()
    {

        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        RaycastHit playerHit;

        if (Physics.Raycast(playerRay, out playerHit))
        {
            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                currentCube = playerHit.transform;

                if (playerHit.transform.GetComponent<Walkable>().isStair)
                {
                    DOVirtual.Float(GetBlend(), blend, .1f, SetBlend);
                    //DOTVirtual.Float(float from,float to,float durationTime, Callback);
                }
                else
                {
                    DOVirtual.Float(GetBlend(), 0, .1f, SetBlend);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.GetChild(0).position, -transform.up);
        Gizmos.DrawRay(ray);
    }

    float GetBlend()
    {
        return GetComponentInChildren<Animator>().GetFloat("Blend");
    }
    void SetBlend(float x)
    {
        GetComponentInChildren<Animator>().SetFloat("Blend", x);
    }

}
