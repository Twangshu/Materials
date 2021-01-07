using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIPanel : MonoBehaviour
{
    private EventDefine showType;
    private EventDefine hideType;
    public EventDefine ShowType { get => showType; set => showType = value; }
    public EventDefine HideType { get => hideType; set => hideType = value; }
    private Vector2 initPos;
    private Vector2 hidePos = new Vector2(1000, 1000);
    private bool isOn = true;

    public virtual void Awake()
    {
        initPos = transform.position;
        EventCenter.AddListener(ShowType, ShowThis);
        EventCenter.AddListener(HideType, HideThis);
    }

    public virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HideThis();
        }
    }

    protected virtual void OnDestroy()
    {
        EventCenter.RemoveListener(ShowType, ShowThis);
        EventCenter.RemoveListener(HideType, HideThis);
    }

    public virtual void ShowThis()
    {
        //if (isOn)
        //    return;
        //isOn = true;
        AudioManager.Instance.PlayClickEffect();
        gameObject.SetActive(true);
      //  transform.position = initPos;
        UIManager.Instance.PushPanel(this);
        if(ShowType!=EventDefine.ShowMainPanel)
        {
            EventCenter.Broadcast(EventDefine.HideMainPanel);
        }
    }

    public virtual void HideThis()
    {
        //if (!isOn)
        //    return;
        //isOn = false;
        gameObject.SetActive(false);
      //  transform.position = hidePos;
        UIManager.Instance.PopPanel();
        if(HideType!=EventDefine.HideMainPanel)
        {
            EventCenter.Broadcast(EventDefine.ShowMainPanel);
        }
    }
}
