using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public Vector3 Forward { get => forward; set => forward = value; }
    public bool CanOprate { get => canOprate; set => canOprate = value; }
    [SerializeField]
    private bool canOprate = true;
    private Vector3 forward =new Vector3 (0, 0, 1);
    private Camera mainCamera;

    public GameObject mapCanvas;
    private GameObject player;

    public override void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.Player);
    }
    public void Start()
    {
        mainCamera = Camera.main;
        
    }


    private void OnDestroy()
    {
        
    }
    public override void Update()
    {
        //forward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z).normalized;
        //UnityEngine.Debug.Log("update");
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    OpenOrCloseMap();
        //}

    }

    public void OpenOrCloseMap()
    {
        mapCanvas.gameObject.SetActive(!mapCanvas.gameObject.activeSelf);
        if (mapCanvas.gameObject.activeSelf)
        {
            mainCamera.gameObject.SetActive(false);
            canOprate = false;
        }
        else
        {
            mainCamera.gameObject.SetActive(true);
            canOprate = true;
        }
    }
}
