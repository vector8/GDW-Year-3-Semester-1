using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public Text timerText;
    public bool paused = false;

    public float timer = 30f;

    // Update is called once per frame
    void Update()
    {
        if(!paused && timer > 0f)
        {
            timer -= Time.deltaTime;
            if(timer < 0f)
            {
                timer = 0f;
            }
            timerText.text = ((int)timer).ToString();
        }
    }

    public void resetTimer()
    {
        timer = 30f;
    }
}
