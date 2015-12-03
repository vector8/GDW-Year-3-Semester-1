using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class StreamExtension : EditorWindow
{
    public static GameObject screenCapObject;

    [MenuItem("Streaming Extension/Streaming Control Panel")]
    public static void showWindow()
    {
        EditorWindow.GetWindow(typeof(StreamExtension));
    }

    void OnGUI()
    {
        if(screenCapObject == null)
        {
            screenCapObject = GameObject.Find("ScreenCapture");
        }
        
        bool active = EditorGUILayout.Toggle("Enable Streaming", screenCapObject.activeSelf);
        screenCapObject.SetActive(active);

        if(GUILayout.Button("Install DLL"))
        {
            //Debug.Log(Application.dataPath.Replace('/', '\\') + @"\StreamingPlugin\Install\SharedMemoryDLL.dll", @"C:\Windows\System32\SharedMemoryDLL.dll");
            File.Copy(Application.dataPath.Replace('/', '\\') + @"\StreamingPlugin\Install\SharedMemoryDLL.dll", @"C:\Windows\System32\SharedMemoryDLL.dll", true);
        }
    }
}
