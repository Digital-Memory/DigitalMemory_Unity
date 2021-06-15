using UnityEngine;
using System.Runtime.InteropServices;
using System;

public static class WebCommunicator
{

    [DllImport("__Internal")]
    private static extern void EnterTime(int time);

    [DllImport("__Internal")]
    private static extern void EnterCloseup(string name);

    [DllImport("__Internal")]
    private static extern void LeaveCloseup(string name);

    public static void SetTime(int time)
    {
#if !UNITY_EDITOR
        EnterTime(time);
#endif
    }
    internal static void ZoomIn(string name)
    {
#if !UNITY_EDITOR
        EnterCloseup(name);
#endif
    }

    internal static void ZoomOut(string name)
    {
#if !UNITY_EDITOR
        LeaveCloseup(name);
#endif
    }
}