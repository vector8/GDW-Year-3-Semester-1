using UnityEngine;
using UnityEditor;
using System.Collections;

public class IgnoreFileEditor : EditorWindow
{
    public static string ignoreFileContents;
    public static string filePath;

    public static void showWindow()
    {
        EditorWindow.GetWindow(typeof(IgnoreFileEditor));
    }

    void OnGUI()
    {
        EditorStyles.textField.wordWrap = true;
        ignoreFileContents = EditorGUILayout.TextArea(ignoreFileContents, GUILayout.ExpandHeight(true));
        EditorStyles.textField.wordWrap = false;

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Save"))
        {
            System.IO.File.WriteAllText(filePath, ignoreFileContents);
            Close();
            HgExtension.checkRepoStatus();
        }
        
        if(GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
}
