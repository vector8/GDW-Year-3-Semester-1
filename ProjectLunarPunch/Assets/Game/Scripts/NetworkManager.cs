using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

public static class NetworkWrapper
{
    private const string DLL_NAME = "NetworkPlugin";

    [DllImport(DLL_NAME)]
    public static extern void setServer(string SERVER);

    [DllImport(DLL_NAME)]
    public static extern int initializeClient();

    [DllImport(DLL_NAME)]
    public static extern int initializeServer();

    [DllImport(DLL_NAME)]
    public static extern int receive();

    [DllImport(DLL_NAME)]
    public static extern int sendTo(string msg);

    [DllImport(DLL_NAME)]
    public static extern System.IntPtr readBuffer();

    [DllImport(DLL_NAME)]
    public static extern int getLength();
    [DllImport(DLL_NAME)]
    public static extern void clearBuffer();

    public static string getLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "0.0.0.0";
    }
}