using UnityEngine;
using System.Collections;

public class Timer
{
    private float time = 0f;

    public void setTime(float t)
    {
        time = t;
    }

    public void update(float elapsed)
    {
        time -= elapsed;
        if(time < 0f)
        {
            time = 0f;
        }
    }

    public bool isDone()
    {
        return Mathf.Approximately(time, 0f);
    }
}
