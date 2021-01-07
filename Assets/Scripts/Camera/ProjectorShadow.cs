using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorShadow : MonoBehaviour
{
    private Camera mCam;
    private RenderTexture mRenderTex;
    private GameObject player;
    public int ratio = 128;

    public LayerMask shadowMask;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.Player);
        Projector pjt = GetComponent<Projector>();
        //创建Camera和RenderTexture
        mCam = gameObject.AddComponent<Camera>();
        mCam.orthographic = pjt.orthographic;
        mCam.orthographicSize = pjt.orthographicSize;
        mCam.cullingMask = shadowMask;
        mCam.clearFlags = CameraClearFlags.SolidColor;
        mCam.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        mRenderTex = new RenderTexture(ratio, ratio, 0, RenderTextureFormat.ARGB32);
        mRenderTex.antiAliasing = 4;//抗锯齿

        mCam.enabled = false;
        mCam.SetReplacementShader(Shader.Find("Custom/ProjectorShadow"), null);
        mCam.targetTexture = mRenderTex;
        pjt.material.SetTexture("_ShadowTex", mRenderTex);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + Vector3.up * 5.6f;
        mCam.Render();
    }

    private void OnApplicationQuit()
    {
        GetComponent<Projector>().material.SetTexture("_ShadowTex", null);
    }
}
