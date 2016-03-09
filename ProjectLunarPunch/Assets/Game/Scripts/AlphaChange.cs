using UnityEngine;
using System.Collections;

public class AlphaChange : MonoBehaviour
{
    public Material material;
    public float fadeInterval;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (material.color.a != 255)
        {
            material.color = new Color(255, 255, 255, Mathf.Lerp(0,255, 0.005f));
        }
    }
}
