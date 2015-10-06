﻿using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenuGroup, vsAiGroup, sparGroup;

    public float onScreenX, offScreenX;

    public float moveTime;

    private float currentMoveTime = 0f;

    private void moveGroups(GameObject inFocus, GameObject outOfFocus1, GameObject outOfFocus2)
    {
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
}
