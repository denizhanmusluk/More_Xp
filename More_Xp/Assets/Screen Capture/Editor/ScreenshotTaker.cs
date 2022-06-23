using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenshotTaker : EditorWindow
{
    public static string lastScreenshot = "";

    [MenuItem("Tools/Screenshot Taker")]
    public static void ShowWindow()
    {
        EditorWindow editorWindow = EditorWindow.GetWindow(typeof(ScreenshotTaker));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.title = "Screenshot";
    }
    
    void OnGUI()
    {
        EditorGUILayout.Space();

        if (GUILayout.Button("Take Screenshot", GUILayout.MinHeight(60)))
            TakeSS();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Open Last SS", GUILayout.MinHeight(40)))
            if (!string.IsNullOrEmpty(lastScreenshot))
            {
                string openPath = "";
                if (string.IsNullOrEmpty(UtilScript.ssFolder))
                    openPath = (Application.dataPath + lastScreenshot).Replace("Assets", "");
                else
                    openPath = UtilScript.ssFolder + "/" + lastScreenshot;
                Debug.Log(openPath);
                Application.OpenURL("file://" + openPath);
            }

        if (GUILayout.Button("Open Folder", GUILayout.MinHeight(40)))
        {
            string openPath = "";
            if (string.IsNullOrEmpty(UtilScript.ssFolder))
                openPath = (Application.dataPath).Replace("Assets", "");
            else
                openPath = UtilScript.ssFolder;
            Debug.Log(openPath);
            Application.OpenURL("file://" + openPath);
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Save Path", EditorStyles.boldLabel);
        EditorGUILayout.TextField(UtilScript.ssFolder);
        GUILayout.Space(15);
        if (GUILayout.Button("Browse"))
            UtilScript.ssFolder = EditorUtility.SaveFolderPanel("Path to Save Images", UtilScript.ssFolder, Application.dataPath);
    }

    public static void TakeSS()
    {
        lastScreenshot = UtilScript.TakeSS();
    }
}