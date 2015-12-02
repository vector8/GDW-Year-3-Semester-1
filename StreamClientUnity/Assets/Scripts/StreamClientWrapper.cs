using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;

public class StreamClientWrapper : MonoBehaviour
{
    public RawImage screenImage;
    private float frameGetTimer = 0f;
    private const float FRAME_GET_DELAY = 0.06f;
    private Texture2D renderTarget;

    private const string DLL_NAME = "SharedMemoryDLL";

    [DllImport(DLL_NAME)]
    private static extern byte[] getFrame();

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        renderTarget = new Texture2D(640, 360, TextureFormat.RGB24, false);
    }

    void getFrameFromStream()
    {
        frameGetTimer += Time.deltaTime;
        if (frameGetTimer >= FRAME_GET_DELAY)
        {
            frameGetTimer = 0f;

            byte[] data = getFrame();

            Color[] colors = renderTarget.GetPixels();

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i].b = ((float)data[i * 3]) / 255f;
                colors[i].r = ((float)data[(i * 3) + 1]) / 255f;
                colors[i].g = ((float)data[(i * 3) + 2]) / 255f;
            }

            renderTarget.SetPixels(colors);
            renderTarget.Apply();

            screenImage.texture = renderTarget;
        }
    }

    void Update()
    {
        getFrameFromStream();
    }
}
