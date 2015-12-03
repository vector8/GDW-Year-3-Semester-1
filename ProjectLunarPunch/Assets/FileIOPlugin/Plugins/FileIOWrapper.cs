using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class FileIOWrapper
{
    private const string DLL_NAME = "FileIOPlugin";

    [DllImport(DLL_NAME)]
    private static extern void saveAscii(string path, string data, bool append);
    [DllImport(DLL_NAME)]
    private static extern void saveBinary(string path, string data, bool append);
    [DllImport(DLL_NAME)]
    private static extern System.IntPtr loadAscii(string path);
    [DllImport(DLL_NAME)]
    private static extern System.IntPtr loadBinary(string path);

    public static string loadFile(string path, bool asBinary)
    {
        if(asBinary)
        {
            return Marshal.PtrToStringAnsi(loadBinary(path));
        }
        else
        {
            return Marshal.PtrToStringAnsi(loadAscii(path));
        }
    }

    public static void saveFile(string path, string data, bool asBinary, bool append)
    {
        if (asBinary)
        {
            saveBinary(path, data, append);
        }
        else
        {
            saveAscii(path, data, append);
        }
    }
}
