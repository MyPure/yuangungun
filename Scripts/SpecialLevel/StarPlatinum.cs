using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPlatinum : MonoBehaviour
{    
    public GameObject player;
    public FlyTrap[] flyTraps;
    public SawTurn[] saws;
    public Animator[] animators;
    public float speed;
    private float timer = 0;
    private float change = 0.02f;
    private bool hasTimeStopped = false;
    private float speedScale = 1;

    private void Accelerate()
    {
        if (Input.GetKey(KeyCode.K))
        {
            speedScale = 1.6f;
            speed = speed * speedScale;
        }
    }

    private void TimeStop(bool state)
    {
        for (int i = 0; i < saws.Length; i++)
        {
            saws[i].enabled = state;
        }
        for (int i = 0; i < flyTraps.Length; i++)
        {
            flyTraps[i].enabled = state;
        }
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = state;
        }
    }

    private void StopTime()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (hasTimeStopped)
            {
                timer = 0;
            }
            hasTimeStopped = !hasTimeStopped;
        }
    }
    private void Update()
    {
        StopTime();
        if (hasTimeStopped)
        {
            timer += change;
        }
    }

}
