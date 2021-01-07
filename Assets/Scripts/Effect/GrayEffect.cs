using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayEffect : PostEffectsBase
{
    [ExecuteInEditMode]

    public Camera sourceCamera;
    public RenderTexture renderTexture;
    public Material material;
    public Shader grayShader;
    public float grayScaleAmout = 1.0f;

    private void Awake()
    {
        sourceCamera = Camera.main;
    }
    public override bool CheckResources()
    {

        material = CheckShaderAndCreateMaterial(grayShader, material);
        if(renderTexture==null)
        {
            renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);

        }
        if (!isSupported)
            ReportAutoDisable();
        return isSupported;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (CheckResources() == false)
        {
            Graphics.Blit(source, destination);
            return;
        }

        if (material != null)
        {
           // material.SetTexture("_OutLine", RenderTexture);
            material.SetFloat("_LuminosityAmount", grayScaleAmout);
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
