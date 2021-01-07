using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cloudtian;

public class UIManager:Singleton<UIManager>
{
    private Stack<BaseUIPanel> currentPanels = new Stack<BaseUIPanel>();
    private Dictionary<string, GameObject> cachaPanel = new Dictionary<string, GameObject>();
    private string cachePath;

    public void LoadPanel(string path)
    {
        
        ResManager.Instance.LoadAsync(path,LoadComplete);
        
    }

    public void LoadComplete(object panel)
    {
        var UIPanel = panel as GameObject;
        var go = GameObject.Instantiate(UIPanel, GameObject.Find("Canvas").transform);
        cachaPanel.Add(cachePath, UIPanel);
        var panelScr = (UIPanel).GetComponent<BaseUIPanel>();
        EventCenter.Broadcast(panelScr.ShowType);
    }
    public void PushPanel(string path)
    {
        if(cachaPanel.ContainsKey(path))
        {
            cachaPanel.TryGetValue(path, out var panel);
            var panelScr = panel.GetComponent<BaseUIPanel>();
           // currentPanels.Push(panelScr);
            panel.transform.SetAsLastSibling();
          //  panel.GetComponent<BaseUIPanel>().ShowThis();
            EventCenter.Broadcast(panelScr.ShowType);
        }
        else
        {
            LoadPanel(path);
        }
    }

    public void PopPanel()
    {
        var panel = currentPanels.Pop();
        if(currentPanels.Count==0)
        {
            EventCenter.Broadcast(EventDefine.ShowMainPanel);
        }
       // EventCenter.Broadcast(panel.HideType);
    }

    public void PushPanel(BaseUIPanel panel)
    {
        currentPanels.Push(panel);
    }

    public bool checkMainPanel()
    {
        return currentPanels.Count == 1;
    }
    

}