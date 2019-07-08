using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheWorld : MonoBehaviour
{
    public GameObject clock;
    public Text clockText;
    public float duration;
    public SawTurn[] saws;
    public BoxCollider2D[] boxes;
    public GameObject effect;
    public Animator[] animators;
    public Animator myAnimation;
    private bool clockRun = false;
    [SerializeField]private float timer = 0;
    [SerializeField]private float change = 0;
    [SerializeField]private bool isPlayerEnter = false;
    [SerializeField] private bool hasTimeStopped = false;

    private void TimeStop(bool state)
    {
        for (int i = 0; i < saws.Length; i++)
        {
            saws[i].enabled = state;
        }
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].isTrigger = state;
        }
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = state;
        }
        myAnimation.SetBool("TimeStop", !state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        clockRun = false;
        clock.SetActive(false);
        change = 0.02f;
        isPlayerEnter = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (hasTimeStopped)
        {
            clockRun = true;
            clock.SetActive(true);
        }

        change = -0.02f;
        isPlayerEnter = false;
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && isPlayerEnter)
        {
            timer += change;
            myAnimation.SetBool("StopTiming", false);
            myAnimation.SetBool("Timing", true);
        }else if ((Input.GetKeyUp(KeyCode.Q) || !isPlayerEnter) && !hasTimeStopped)
        {
            timer = 0;
            myAnimation.SetBool("Timing", false);
            myAnimation.SetBool("StopTiming", true);
        }

        if (hasTimeStopped)
        {
            if(clockRun) clockText.text = Convert.ToString(duration+timer);
            timer += change;
        }
        if (timer > 3)
        {            
            hasTimeStopped = true;
            TimeStop(false);
            timer = 0;
        }else if (timer < -duration)
        {
            hasTimeStopped = false;
            clock.SetActive(false);
            TimeStop(true);
            change = 0;
            timer = 0;
        }
    }
}
