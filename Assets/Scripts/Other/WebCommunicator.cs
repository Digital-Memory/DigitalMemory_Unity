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
        EnterTime(time);
    }
    internal static void ZoomIn(int id)
    {
        EnterCloseup(id);
    }

    internal static void ZoomOut()
    {
        LeaveCloseup();
    }
}