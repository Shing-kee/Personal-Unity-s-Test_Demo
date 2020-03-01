using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer 
{
    public enum STATE
    {
        IDLE,
        RUN,
        FINISHED
    }
    public STATE state;

    public float duration = 1.0f;

    private float elapsedTime = 0;

    public void Tick()
    {
        switch (state)
        {
            case STATE.IDLE:
                break;
            case STATE.RUN:
                if (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                }
                else
                {
                    state = STATE.FINISHED;
                }
                break;
            case STATE.FINISHED:
                break;
            default:Debug.LogWarning("MyTimer Error!");
                break;
        }
    }
    public void Go()
    {
        elapsedTime = 0;
        state = STATE.RUN;
    }
}
