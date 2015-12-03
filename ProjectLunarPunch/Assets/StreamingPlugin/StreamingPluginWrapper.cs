using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;

public class StreamingPluginWrapper : MonoBehaviour
{
    public RawImage test;

    private float frameCapTimer = 0f;
    private const float FRAME_CAP_DELAY = 0.06f;
    private Texture2D frameTexture;
    private byte[] data = new byte[691200];

    private const string DLL_NAME = "SharedMemoryDLL";

	[DllImport(DLL_NAME)]
    private static extern void setFrame(byte[] data);//, int size);

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        frameTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    void LateUpdate()
    {
        if(frameTexture.width != Screen.width || frameTexture.height != Screen.height)
        {
            frameTexture.Resize(Screen.width, Screen.height);
        }
        StartCoroutine(captureFrame());
    }

    IEnumerator captureFrame()
    {
        yield return new WaitForEndOfFrame();

        frameCapTimer += Time.deltaTime;
        if(frameCapTimer >= FRAME_CAP_DELAY)
        {
            float startTime = Time.realtimeSinceStartup;
            frameCapTimer = 0f;

            // Grab frame from screen.
            frameTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            frameTexture.Apply();

            TextureScale.Point(frameTexture, 640, 360);

            Color[] colors = frameTexture.GetPixels();
            for (int i = 0; i < colors.Length; i++)
            {
                data[i * 3] = (byte)(colors[i].r * 255f);
                data[(i * 3) + 1] = (byte)(colors[i].g * 255f);
                data[(i * 3) + 2] = (byte)(colors[i].b * 255f);
            }

            setFrame(data);
            //testSetScreenFrame(data);     // Used for debugging to send the captured frame to a texture in this application

            float endTime = Time.realtimeSinceStartup;
            //Debug.Log("Frame cap took: " + (endTime - startTime).ToString() + " seconds.");
        }
    }

    private void testSetScreenFrame(byte[] data)
    {
        Texture2D tex = new Texture2D(640, 360, TextureFormat.RGB24, false);
        Color[] colors = new Color[230400];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].r = ((float)data[i * 3]) / 255f;
            colors[i].g = ((float)data[(i * 3) + 1]) / 255f;
            colors[i].b = ((float)data[(i * 3) + 2]) / 255f;
        }

        tex.SetPixels(colors);
        tex.Apply();

        test.texture = tex;
    }
}
