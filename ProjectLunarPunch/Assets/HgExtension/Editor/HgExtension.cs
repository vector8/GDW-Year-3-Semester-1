using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class HgExtension : EditorWindow
{
    private string commitMessage = "";
    public static HgPluginWrapper.RepoStatus repoStatus = HgPluginWrapper.RepoStatus.NotSet;
    private static string localPath, remotePath;
    private static bool localChanged = false, remoteChanged = false;

    [MenuItem("Hg Extension/Hg Control Panel")]
    public static void showWindow()
    {
        EditorWindow.GetWindow(typeof(HgExtension));
    }

    void OnGUI()
    {

        if (localPath == null)
        {
            localPath = "";
        }
        if (remotePath == null)
        {
            remotePath = "";
        }

        GUILayout.Space(10f);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Status: ");
        GUIStyle statusStyle = new GUIStyle(GUI.skin.label);
        switch (repoStatus)
        {
            case HgPluginWrapper.RepoStatus.Clean:
                statusStyle.normal.textColor = new Color(0f, 0.75f, 0f);
                GUILayout.Label("Clean", statusStyle);
                break;
            case HgPluginWrapper.RepoStatus.Dirty:
                statusStyle.normal.textColor = Color.red;
                GUILayout.Label("Changed", statusStyle);
                break;
            case HgPluginWrapper.RepoStatus.NotSet:
            default:
                statusStyle.normal.textColor = Color.red;
                GUILayout.Label("No Repo Set", statusStyle);
                break;
        }
        if (GUILayout.Button("Check Now"))
        {
            checkRepoStatus();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Label("Local: ");
        GUILayout.Label("Remote: ");
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUI.SetNextControlName("LocalField");
        string tempLocal = EditorGUILayout.TextField(localPath);
        bool focusRemote = false;
        if (GUILayout.Button("Browse"))
        {
            tempLocal = EditorUtility.OpenFolderPanel("Set local repository path", "", "");
            if (tempLocal.Length > 0)
            {
                localPath = tempLocal;
                HgPluginWrapper.runCommand(HgPluginWrapper.CommandType.SetLocal, localPath);
                if (!Directory.Exists(localPath + "/.hg"))
                {
                    // display a prompt to create a repo
                    if (EditorUtility.DisplayDialog("Create Repo?", "No repository was found at the selected path, would you like to create one now?", "Yes", "No"))
                    {
                        HgPluginWrapper.runCommand(HgPluginWrapper.CommandType.Create);
                    }
                }
            }
            focusRemote = true;
        }
        else if (tempLocal != localPath)
        {
            localChanged = true;
            localPath = tempLocal;
            HgPluginWrapper.runCommand(HgPluginWrapper.CommandType.SetLocal, localPath);
        }
        if (localChanged && GUI.GetNameOfFocusedControl() != "LocalField")
        {
            localChanged = false;
            checkRepoStatus();
        }
        GUILayout.EndHorizontal();
        GUI.SetNextControlName("RemoteField");
        string tempRemote = EditorGUILayout.TextField(remotePath);
        if(tempRemote != remotePath)
        {
            remoteChanged = true;
            remotePath = tempRemote;
            HgPluginWrapper.runCommand(HgPluginWrapper.CommandType.SetRemote, remotePath);
        }
        if (remoteChanged && GUI.GetNameOfFocusedControl() != "RemoteField")
        {
            remoteChanged = false;
            checkRepoStatus();
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        if(focusRemote)
        {
            GUI.FocusControl("RemoteField");
        }

        GUI.enabled = (localPath.Length > 0);
        
        if(GUILayout.Button("Edit Ignore File"))
        {
            string ignoreFilePath = localPath + "/.hgignore";
            IgnoreFileEditor.filePath = ignoreFilePath;
            FileStream fileStream = new FileStream(ignoreFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

            using (StreamReader reader = new StreamReader(fileStream))
            {
                IgnoreFileEditor.ignoreFileContents = reader.ReadToEnd();
            }

            fileStream.Close();
            IgnoreFileEditor.showWindow();
        }

        GUILayout.Space(10f);

        GUI.enabled = (repoStatus != HgPluginWrapper.RepoStatus.NotSet);

        if (GUILayout.Button("Add New Files", GUILayout.MaxWidth(100f)))
        {
            HgPluginWrapper.runCommand(HgPluginWrapper.CommandType.Add);
            checkRepoStatus();
        }

        GUILayout.Space(10f);

        GUI.enabled = (repoStatus == HgPluginWrapper.RepoStatus.Dirty);

        GUILayout.Label("Commit message:");
        GUI.SetNextControlName("CommitMessage");
        EditorStyles.textField.wordWrap = true;
        commitMessage = EditorGUILayout.TextArea(commitMessage, GUILayout.Height(40f));
        EditorStyles.textField.wordWrap = false;

        bool clicked = false;
        GUI.SetNextControlName("Commit");
        if (GUILayout.Button("Commit", GUILayout.MaxWidth(60f)))
        {
            // send the commit message to the plugin and commit.
            HgPluginWrapper.runCommand(HgPluginWrapper.CommandType.Commit, commitMessage);
            commitMessage = "";
            GUI.FocusControl("Commit");
            clicked = true;
            checkRepoStatus();
        }

        if(!clicked && GUI.GetNameOfFocusedControl() == "Commit")
        {
            GUI.FocusControl("CommitMessage");
        }

        GUILayout.Space(10f);

        GUI.enabled = (repoStatus != HgPluginWrapper.RepoStatus.NotSet);
        if (GUILayout.Button("Update To Latest"))
        {
            HgPluginWrapper.runCommand(HgPluginWrapper.CommandType.Update);
            checkRepoStatus();
        }

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Pull"))
        {
            HgPluginWrapper.runCommand(HgPluginWrapper.CommandType.Pull);
        }
        if(GUILayout.Button("Push"))
        {
            HgPluginWrapper.runCommand(HgPluginWrapper.CommandType.Push);
        }
        GUILayout.EndHorizontal();
        GUI.enabled = true;
    }

    public static void checkRepoStatus()
    {
        if (localPath.Length == 0 || remotePath.Length == 0)
        {
            repoStatus = HgPluginWrapper.RepoStatus.NotSet;
        }
        else if (HgPluginWrapper.hasChanged())
        {
            repoStatus = HgPluginWrapper.RepoStatus.Dirty;
        }
        else
        {
            repoStatus = HgPluginWrapper.RepoStatus.Clean;
        }
    }
}