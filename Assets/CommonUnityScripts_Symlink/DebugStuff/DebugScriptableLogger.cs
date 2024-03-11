using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugScriptableLogger", menuName = "ScriptableObjects/DebugScriptableLogger")]
public class DebugScriptableLogger : ScriptableObject
{
    public string loggerName;
    public virtual void LogString(string item)
    {
        Debug.Log(loggerName + item);
    }
    
}
