using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using UnityEngine;

public class BaseNPC : MonoBehaviour
{
    protected int id;
    protected string npcName;
    protected string[] dialogs;//对话内容
    protected string[] selections;//选项
    protected List<Task> tasks;//任务内容

   
    public virtual void ShowDialogs()
    {
        
    }

    public virtual void ExitDialogs()
    {
        EventCenter.Broadcast(EventDefine.ExitDialogs);
        Log.LogDebug("exitDialogs");
    }
}
