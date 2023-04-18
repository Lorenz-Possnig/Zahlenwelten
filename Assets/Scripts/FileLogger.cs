using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileLogger : MonoBehaviour
{
    private string Filename = "";

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
        Debug.Log($"Starting to write logs to {Filename}");
    }

    private void OnDisable()
    {
        Debug.Log($"Stopping to write logs to {Filename}");
        Application.logMessageReceived -= Log;
    }

    // Start is called before the first frame update
    void Start()
    {
        Filename = Application.dataPath + "/Logs.txt";
    }

    public void Log(string logString, string stackTrace, LogType logType)
    {
        TextWriter tw = new StreamWriter(Filename, true);
        tw.WriteLine($"[{DateTime.Now}] {logString}");
        if (logType == LogType.Error || logType == LogType.Exception)
        {
            tw.WriteLine(stackTrace);
        }
        tw.Close();
    }
}
