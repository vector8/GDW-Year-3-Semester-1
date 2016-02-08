﻿using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenuGroup, vsAiGroup, sparGroup;

    public float moveTime;

    private Camera mainCam;
    private Vector3 armyBuildPos;

    private SplineWalker walker;

    //private float currentMoveTime = 0f;

    void Awake()
    {
        MouseInputWrapper.initialize();
        armyBuildPos.x = 0f;
        armyBuildPos.y = 0f;
        armyBuildPos.z = 0f;
        walker = GameObject.Find("Main Camera").GetComponent<SplineWalker>();
    }

    private void moveGroups(GameObject inFocus, GameObject outOfFocus1, GameObject outOfFocus2)
    {
        float onScreenX = Camera.main.pixelWidth / 2f;
        float offScreenX = 2000f;

        Vector3 pos = inFocus.transform.position;
        pos.x = onScreenX;
        inFocus.transform.position = pos;

        pos = outOfFocus1.transform.position;
        pos.x = offScreenX;
        outOfFocus1.transform.position = pos;

        pos = outOfFocus2.transform.position;
        pos.x = offScreenX;
        outOfFocus2.transform.position = pos;
    }

    public void goToMainMenuGroup()
    {
        moveGroups(mainMenuGroup, vsAiGroup, sparGroup);
    }

    public void goToVsAiGroup()
    {
        moveGroups(vsAiGroup, mainMenuGroup, sparGroup);
    }

    public void goToSparGroup()
    {
        moveGroups(sparGroup, mainMenuGroup, vsAiGroup);
    }

    public void goToArmySelect()
    {
        walker.enabled = true;
        mainMenuGroup.SetActive(false);

        //enable for demo if one scene not done.
        //Application.LoadLevel("ArmyBuilder2");
    }

    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    void Update()
    {

    }

    public void goToVsAiScene()
    {
        walker.spline = GameObject.Find("Army -> Combat Spline").GetComponent<BezierSpline>();
        walker.lookBehind = false;
        walker.lookForward = true;
        walker.enabled = true;

        if (walker.progress == 1f)
        Application.LoadLevel("Battle");
    }
}
