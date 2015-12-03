using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class MouseInputWrapper
{
    private const string DLL_NAME = "MouseInputDLL";

    private static bool initialized = false;

    public enum MouseButtons
    {
        Left,
        Middle,
        Right
    }

    [DllImport("user32.dll")]
    private static extern System.IntPtr GetActiveWindow();

    [DllImport(DLL_NAME)]
    private static extern bool setHookTarget(System.IntPtr wHnd);

    [DllImport(DLL_NAME)]
    private static extern void shutdownMousePlugin();

    [DllImport(DLL_NAME)]
    private static extern int getMouseX();

    [DllImport(DLL_NAME)]
    private static extern int getMouseY();

    [DllImport(DLL_NAME)]
    private static extern bool isMouseButtonDown(int buttonID);

    [DllImport(DLL_NAME)]
    private static extern bool isMouseButtonDownFirstTime(int buttonID);

    public static void initialize()
    {
        setHookTarget(GetActiveWindow());
        initialized = true;
    }

    public static Vector2 getMousePosition()
    {
#if UNITY_EDITOR
        return Input.mousePosition;
#elif UNITY_STANDALONE
        if(!initialized)
        {
            initialize();
        }

        Vector2 position = new Vector2();

        position.x = getMouseX();
        position.y = Screen.height - getMouseY();

        return position;
#endif
    }

    public static bool GetMouseButton(MouseButtons button)
    {
        if (!initialized)
        {
            initialize();
        }
        
        int id = (int)button;
        return isMouseButtonDown(id);
    }

    public static bool GetMouseButtonDown(MouseButtons button)
    {
        if (!initialized)
        {
            initialize();
        }
        
        int id = (int)button;
        return isMouseButtonDownFirstTime(id);
    }
}
