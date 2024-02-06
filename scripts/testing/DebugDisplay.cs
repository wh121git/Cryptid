using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplay : MonoBehaviour
{
    public GameObject text;

    // [1] Basic log debug message documentation https://docs.unity3d.com/ScriptReference/Application-logMessageReceived.html
    public string debugOutput = "";
    public string debugStack = "";

    public string ownOutput = "";

    public bool fullDebug = false;
    public bool ownDebug = false;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        debugOutput += "||" + logString;
        debugStack += "||" + stackTrace;
    }
    

    private void Update()
    {
        if (fullDebug)
        {
            text.GetComponent<Text>().text = debugOutput;
        }
        if (ownDebug)
        {
            text.GetComponent<Text>().text = ownOutput;
        }
    }

    public void clearDebug()
    {
        debugOutput = string.Empty;
    }

    public void addOut(string output)
    {
        ownOutput += "|| " + output;
    }
}
