using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class HgPluginWrapper
{
    private const string DLL_NAME = "HgPluginDLL";

    public enum CommandType
    {
        Add,
        Commit,
        Create,
        Error,
        Forget,
        Pull,
        Push,
        Remove,
        SetLocal,
        SetRemote,
        Update,
        NUMBER_COMMAND_TYPES
    };

    public enum RepoStatus
    {
        Clean,
        Dirty,
        NotSet
    };

    [DllImport(DLL_NAME)]
    private static extern void runCommand(int commandType, string msg);
    [DllImport(DLL_NAME)]
    public static extern bool hasChanged();
    [DllImport(DLL_NAME)]
    private static extern bool getErrorStatus();
    [DllImport(DLL_NAME)]
    private static extern System.IntPtr getErrorMessage();

    public static string runCommand(CommandType t, string arg = "")
    {
        string errorMessage = "";

        runCommand((int)t, arg);

        if(getErrorStatus())
        {
            errorMessage = Marshal.PtrToStringAnsi(getErrorMessage());
        }

        return errorMessage;
    }
}