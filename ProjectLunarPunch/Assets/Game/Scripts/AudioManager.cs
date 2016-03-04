using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    
    void Start()
    {
        DontDestroyOnLoad(GameObject.Find("Eve_of_Battle_Song"));
    }


    void Update()
    {
        if (Application.loadedLevelName == "Battle")
        {
            AudioSource.Destroy(GameObject.Find("Eve_of_Battle_Song"));
        }
    }
}
