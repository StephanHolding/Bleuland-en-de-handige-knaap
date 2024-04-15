using System.Collections.Generic;
using UnityEngine;

public class ScreenLogger : SingletonTemplateMono<ScreenLogger>
{

    private static Queue<string> logQueue = new Queue<string>();

    private GUIStyle style;

    private void Start()
    {
        style = new GUIStyle();
        style.richText = true;
    }

    public static void Log(string message)
    {
        logQueue.Enqueue("<color=white> [Log] " + message + " </color>");
    }

    public static void LogError(string message)
    {
        logQueue.Enqueue("<color=red> [ERROR] " + message + " </color>");
    }

    public static void LogWarning(string message)
    {
        logQueue.Enqueue("<color=yellow> [Warning!] " + message + " </color>");
    }

    private void OnGUI()
    {

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.Label("\n" + string.Join("\n", logQueue.ToArray()), style);
        GUILayout.EndArea();
    }

}
