using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utils
{
    public static Sprite Tex2Sprite(Texture2D tex)
    {
        if (tex == null)
        {
            return null;
        }
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public static Vector2 WorldPosToUIPos(Vector3 worldPos, Canvas canvas)
    {
        //摄像机空间值域[0,1]，z轴值代表深度
        var viewPos = Camera.main.WorldToViewportPoint(worldPos);
        //按照值域进行裁剪
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            //屏幕空间高度值
            float sheight = viewPos.y * Screen.height;
            //屏幕空间宽度值
            float swidth = viewPos.x * Screen.width;
            //适配转化
            return new Vector2(GetFixed(swidth, canvas), GetFixed(sheight, canvas));
        }
        //返回一个固定值-1代表不在屏幕当中
        return -Vector2.one;
    }

    //Screen坐标值适配Canvas画布
    public static float GetFixed(float value, Canvas canvas)
    {
        var cs = canvas.GetComponent<CanvasScaler>();
        if (cs.matchWidthOrHeight == 0)
            //匹配宽度时仅按照宽度计算
            return value * cs.referenceResolution.x / Screen.width;
        else
            //匹配高度时仅按照高度计算
            return value * cs.referenceResolution.y / Screen.height;
    }

    public static float GetDefRate(float def)
    {
        return 1 - def / (def + 1000);
    }

    public static bool isInVisual(Vector3 pos)
    {
        var viewPos = Camera.main.WorldToViewportPoint(pos);
        if (isBetween(viewPos.x, 0, 1) && isBetween(viewPos.y, 0, 1))
            return true;
        return false;
    }

    public static bool isBetween(float value, float min, float max)
    {
        if (min <= value && value <= max)
            return true;
        return false;
    }

    static int resRequestId = 0;
    public static int GenerateRequestId()
    {
        resRequestId++;
        if (resRequestId < 100000000)
            return resRequestId;
        return -1;
    }
}

    public class Tuple<T1, T2>
    {
        public T1 t1;
        public T2 t2;
        public Tuple() { }

        public Tuple(T1 t1, T2 t2)
        {
            this.t1 = t1;
            this.t2 = t2;
        }
        public override bool Equals(object obj)
        {
            var other = (Tuple<T1, T2>)obj;
            return other.t1.Equals(t1) && other.t2.Equals(t2);
        }

        public override int GetHashCode()
        {
            int t1_hashcode = t1.GetHashCode();
            int t2_hashcode = t2.GetHashCode();
            return t1_hashcode + t2_hashcode;
        }
    }

