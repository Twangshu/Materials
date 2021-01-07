using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;

public class PostProcessMgr : Singleton<PostProcessMgr>
{
    private DepthOfField depthOfFieldEffect;
    private ColorCorrectionCurves colorCorrectionEffect;

    public DepthOfField DepthOfFieldEffect { get { 
        if(depthOfFieldEffect==null)
            {
                depthOfFieldEffect = GameObject.FindObjectOfType<DepthOfField>();
            }
            return depthOfFieldEffect;
        } }
    public ColorCorrectionCurves ColorCorrectionEffect { get {
            if (colorCorrectionEffect == null)
            {
                colorCorrectionEffect = GameObject.FindObjectOfType<ColorCorrectionCurves>();
            }
            return colorCorrectionEffect;
        }
    }

    public override void Awake()
    {
        
    }

    //模糊效果
    public void GetBlurEffect()
    {
        depthOfFieldEffect.enabled = true;
        depthOfFieldEffect.focalLength = 0;
        DOTween.To(() => depthOfFieldEffect.focalLength, x => depthOfFieldEffect.focalLength = x, 1000, 5f);
    }

    public void RecoveFromBlur()
    {
        depthOfFieldEffect.enabled = false;
    }

    //显示颜色
    public void CorrectColor(ColorType color)
    {
        switch (color) {
            case ColorType.Green:
                colorCorrectionEffect.isShowGreen = 1;
                break;
            case ColorType.Yellow:
                colorCorrectionEffect.isShowYellow = 1;
                break;
            case ColorType.Blue:
                colorCorrectionEffect.isShowBlue = 1;
                break;
            case ColorType.Red:
                colorCorrectionEffect.isShowRed = 1;
                break;
            case ColorType.All:
                colorCorrectionEffect.isShowAll = 1;
                break;

        }
    }
}
