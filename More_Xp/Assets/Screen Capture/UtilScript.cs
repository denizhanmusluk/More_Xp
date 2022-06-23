using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public static class UtilScript
{

    public static string ssFolder = "";
    public static string TakeSS()
    {
        string path = Screen.width + "x" + Screen.height + "_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss.fff") + ".png";
        ScreenCapture.CaptureScreenshot(string.IsNullOrEmpty(ssFolder) ? path : ssFolder + "/" + path, 1);
        return path;
    }
}