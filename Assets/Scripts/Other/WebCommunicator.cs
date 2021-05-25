using UnityEngine;
using System.Runtime.InteropServices;
using System;

public static class WebCommunicator
{

    [DllImport("__Internal")]
    private static extern void EnterTime(int time);

    [DllImport("__Internal")]
    private static extern void EnterCloseup(int id);

    [DllImport("__Internal")]
    private static extern void LeaveCloseup();

    public static void SetTime(int time)
    {
#if !UNITY_EDITOR
        EnterTime(time);
#endif
    }
    internal static void ZoomIn(int id)
    {
#if !UNITY_EDITOR
        EnterCloseup(id);
#endif
    }

    internal static void ZoomOut()
    {
#if !UNITY_EDITOR
        LeaveCloseup();
#endif
    }
}