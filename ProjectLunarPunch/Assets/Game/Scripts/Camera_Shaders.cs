using UnityEngine;
using System.Collections;

public class Camera_Shaders : MonoBehaviour {
    
    public Camera mainCamera;
    public Shader toonShader;

	void Start () {
        mainCamera.RenderWithShader(toonShader, "RenderType");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
