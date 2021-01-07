using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Log
{
    
    public static void LogError(params object[] contents)
    {
        //Debug.LogError()
    }

    public static void LogDebug(params object[] contents)
    {
        var str = GetContent(contents);
        Debug.Log(str);
    }

    public static void LogWarning(params object[] contents)
    {

    }

    public static void LogAssert(params object[] contents)
    {
        var str = GetContent(contents);
        EditorUtility.DisplayDialog("错误", str, "确定");
    }

    public static void LogHint(params object[] contents)
    {
        var str = GetContent(contents);
        Debug.Log(str);
    }

    private static string GetContent(params object[] contents)
    {
        var content = "";
        foreach (var item in contents)
        {
            content += item.ToString();
        }
        return content;
    }
}
